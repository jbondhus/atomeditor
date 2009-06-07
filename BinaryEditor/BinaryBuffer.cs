using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Kirishima16.Forms
{
	delegate void UndoDelegate();

	internal class BinaryBuffer : IDisposable
	{
		int PAGE_LEN = Properties.Settings.Default.PageLength;

		public event EventHandler<UndoEventArgs> BeforeStackUndo;

		Stream stream;

		public Stream Stream
		{
			get { return stream; }
		}

		LinkedList<BinaryPage> pages;

		bool bReadOnly;
		public bool ReadOnly
		{
			get { return bReadOnly; }
			set { bReadOnly = value; }
		}

		private long length;
		public long Length
		{
			get
			{
				return length;
			}
		}

		bool bFindingCanceled;

		Stack<Stack<UndoDelegate>> undoBuffer = new Stack<Stack<UndoDelegate>>();

		public bool CanUndo {
			get { return undoBuffer.Count > 0; }
		}

		public BinaryBuffer()
		{
			stream = new MemoryStream();
		}

		public BinaryBuffer(Stream s)
		{
			if (!s.CanRead || !s.CanSeek) {
				throw new ArgumentException("読み込みおよびシークの可能なストリームのみでBinaryBufferを初期化できます。");
			}
			if (!s.CanWrite) {
				bReadOnly = true;
			}
			stream = s;

			//ページの論理情報を作成
			length = s.Length;
			pages = new LinkedList<BinaryPage>();
			for (long i = 0; i <= s.Length / PAGE_LEN - 1; i++) {
				pages.AddLast(new BinaryPage(i * PAGE_LEN, PAGE_LEN));
			}
			pages.AddLast(new BinaryPage((s.Length / PAGE_LEN) * PAGE_LEN, (int)(s.Length % PAGE_LEN)));
		}

		LinkedListNode<BinaryPage> lastpage = null;
		int lastidx = -1;
		/// <summary>
		/// 指定されたバイナリを検索します。
		/// </summary>
		/// <param name="target">検索するバイナリ</param>
		/// <param name="start">検索開始位置</param>
		/// <param name="orientation">検索方向が下ならばTrue、上ならばFalse</param>
		/// <returns>最初に見つかった位置、さもなければ-1</returns>
		public long FindBinary(byte[] target, long start, bool orientation)
		{
			if (target.Length < 1) {
				return -1;
			}
			LinkedListNode<BinaryPage> node = orientation ? pages.First : pages.Last;
			bool flg = false;
			int idx = 0;
			while (node != null) {
				if (!node.Value.Removed && node.Value.Position >= start - PAGE_LEN) {
					flg = true;
					idx = checked((int)(start - node.Value.Position));
				}
				if (flg) {
					byte b = 0;
					if (orientation) {
						for (; idx < node.Value.Length; idx++) {
							if (node == pages.Last && idx > node.Value.Length - target.Length) {
								return -1;
							}
							if (bFindingCanceled) {
								bFindingCanceled = false;
								return -2;
							}
							if (node.Value.Modified) {
								b = node.Value.Buffer[idx];
							} else {
								stream.Seek(node.Value.SourcePosition + idx, SeekOrigin.Begin);
								b = (byte)stream.ReadByte();
							}
							if (target[0] == b) {
								if (ArrayEquals(GetBytesAt(node.Value.Position + idx, target.Length), target)) {
									lastpage = node;
									lastidx = idx;
									return node.Value.Position + idx;
								}
							}
						}
					} else {
						for (; idx >= 0; idx--) {
							if (bFindingCanceled) {
								bFindingCanceled = false;
								return -2;
							}
							if (node.Value.Modified) {
								b = node.Value.Buffer[idx];
							} else {
								stream.Seek(node.Value.SourcePosition + idx, SeekOrigin.Begin);
								b = (byte)stream.ReadByte();
							}
							if (target[0] == b) {
								if (ArrayEquals(GetBytesAt(node.Value.Position + idx, target.Length), target)) {
									lastpage = node;
									lastidx = idx;
									return node.Value.Position + idx;
								}
							}
						}
					}
				}
				if (orientation) {
					idx = 0;
					node = node.Next;
				} else {
					node = node.Previous;
					idx = checked((int)(node.Value.Length - 1));
				}
			}
			return -1;
		}

		public void CancelFinding()
		{
			bFindingCanceled = true;
		}

		bool ArrayEquals(Array b1, Array b2)
		{
			if (b1.Length != b2.Length)
				return false;
			for (int i = 0; i < b1.Length; i++) {
				if (!b1.GetValue(i).Equals(b2.GetValue(i))) {
					return false;
				}
			}
			return true;
		}

		private BinaryPage GetPageByPos(long pos)
		{
			BinaryPage page = null;
			if (pos == length) {
				return pages.Last.Value;
			}
			foreach (BinaryPage p in pages) {
				if (!p.Removed && p.Position >= pos - PAGE_LEN) {
					page = p;
					break;
				}
			}
			return page;
		}

		private BinaryPage GetPageByPos(long pos, out IEnumerator<BinaryPage> ie)
		{
			ie = pages.GetEnumerator();
			BinaryPage p = null, page = null;
			while (ie.MoveNext()) {
				p = ie.Current;
				if (!p.Removed && p.Position <= pos && pos < p.Position + p.Length) {
					page = p;
					break;
				}
			}
			if (pos == length) {
				return pages.Last.Value;
			}
			return page;
		}

		public byte GetByteAt(long pos)
		{
			BinaryPage page = GetPageByPos(pos);
			if (page.Modified) {
				return page.Buffer[(int)(pos - page.Position)];
			} else {
				stream.Seek(pos - page.Position + page.SourcePosition, SeekOrigin.Begin);
				return (byte)stream.ReadByte();
			}
		}

		public byte[] GetBytesAt(long pos, long len)
		{
			byte[] buf = new byte[len];
			if (len < 1)
				return buf;
			int tlen = 0, offset = 0;
			foreach (BinaryPage p in pages) {
				if (!p.Removed && p.Position > pos - PAGE_LEN) {
					tlen = (int)Math.Min(len - offset, Math.Min(p.Length - (pos - p.Position), len));
					if (p.Modified) {
						p.Buffer.CopyTo((int)(pos + offset - p.Position), buf, offset, tlen);
					} else {
						stream.Seek(pos + offset - p.Position + p.SourcePosition, SeekOrigin.Begin);
						stream.Read(buf, offset, tlen);
					}
					offset += tlen;

					if (offset >= len)
						break;
				}
			}
			return buf;
		}

		public bool MakeBuffer(BinaryPage page)
		{
			if (!page.Modified) {
				byte[] buf = new byte[page.Length];
				stream.Seek(page.SourcePosition, SeekOrigin.Begin);
				stream.Read(buf, 0, page.Length);
				page.Buffer = new List<byte>(buf);
				return true;
			}
			return false;
		}

		public void RemoveBuffer(BinaryPage page)
		{
			if (page.Modified) {
				page.Buffer = null;
			}
		}

		private void RunBeforeStackUndo(Stack<UndoDelegate> undo)
		{
			if (BeforeStackUndo != null) {
				UndoEventArgs ea = new UndoEventArgs();
				BeforeStackUndo(this, ea);
				if (ea.AdditionalOperation != null) {
					undo.Push(ea.AdditionalOperation);
				}
			}
		}

		public void Insert(byte data, long pos)
		{
			Stack<UndoDelegate> undo = new Stack<UndoDelegate>();
			RunBeforeStackUndo(undo);
			IEnumerator<BinaryPage> ie;
			BinaryPage p = GetPageByPos(pos, out ie);
			if (MakeBuffer(p)) {
				undo.Push(delegate() {
					RemoveBuffer(p);
				});
			}
			p.Buffer.Insert((int)(pos - p.Position), data);
			undo.Push(delegate() {
				p.Buffer.RemoveAt((int)(pos - p.Position));
			});
			while (ie.MoveNext()) {
				ie.Current.Position++;
				undo.Push(delegate() {
					ie.Current.Position--;
				});
			}
			length++;
			undo.Push(delegate() {
				length--;
			});
			undoBuffer.Push(undo);
		}

		public void InsertRange(byte[] data, long pos, int len)
		{
			Stack<UndoDelegate> undo = new Stack<UndoDelegate>();
			RunBeforeStackUndo(undo);
			IEnumerator<BinaryPage> ie;
			BinaryPage p = GetPageByPos(pos, out ie);
			if(MakeBuffer(p)){
				undo.Push(delegate() {
					RemoveBuffer(p);
				});
			}
			p.Buffer.InsertRange((int)(pos - p.Position), data);
			undo.Push(delegate() {
				p.Buffer.RemoveRange((int)(pos - p.Position), data.Length);
			});
			while (ie.MoveNext()) {
				ie.Current.Position += data.Length;
				undo.Push(delegate() {
					ie.Current.Position -= data.Length;
				});
			}
			length += len;
			undo.Push(delegate() {
				length -= len;
			});
			undoBuffer.Push(undo);
		}

		public void Remove(long pos)
		{
			Stack<UndoDelegate> undo = new Stack<UndoDelegate>();
			RunBeforeStackUndo(undo);
			bool flg = false;
			IEnumerator<BinaryPage> ie;
			BinaryPage p = GetPageByPos(pos, out ie);
			if (MakeBuffer(p)) {
				undo.Push(delegate() {
					RemoveBuffer(p);
				});
			}
			if (p.Length > 1) {
				byte data = p.Buffer[(int)(pos - p.Position)];
				p.Buffer.RemoveAt((int)(pos - p.Position));
				undo.Push(delegate() {
					p.Buffer.Insert((int)(pos - p.Position), data);
				});
				flg = true;
			} else {
				p.SetRemoved();
				undo.Push(delegate() {
					p.ResetRemoved();
				});
			}
			while (ie.MoveNext()) {
				ie.Current.Position--;
				undo.Push(delegate() {
					ie.Current.Position++;
				});
				flg = true;
			}
			if (flg == false) {
				pages.AddLast(new BinaryPage(0, 0));
				undo.Push(delegate() {
					pages.RemoveLast();
				});
				long lengthbk = length;
				undo.Push(delegate() {
					length = lengthbk;
				});
				length = 0;
				undoBuffer.Push(undo);
				return;
			}
			length--;
			undo.Push(delegate() {
				length++;
			});
			undoBuffer.Push(undo);
		}

		public void RemoveRange(long pos, long len)
		{
			if (pos - 1 >= length) {
				return;
			}
			Stack<UndoDelegate> undo = new Stack<UndoDelegate>();
			RunBeforeStackUndo(undo);
			int tlen = 0, deleted = 0;
			long slen = len, spos = pos;
			bool flg = false;
			IEnumerator<BinaryPage> ie = pages.GetEnumerator();
			BinaryPage p;
			while (ie.MoveNext()) {
				p = ie.Current;
				if (!p.Removed && p.Position >= pos - PAGE_LEN) {
					tlen = (int)Math.Min(p.Length - (pos - p.Position), len);
					if (tlen == p.Length) {
						p.SetRemoved();
						undo.Push(delegate() {
							p.ResetRemoved();
						});
					} else {
						if (MakeBuffer(p)) {
							undo.Push(delegate() {
								RemoveBuffer(p);
							});
						}
						byte[] databk = new byte[tlen];
						p.Buffer.CopyTo((int)(pos - p.Position), databk, 0, tlen);
						p.Buffer.RemoveRange((int)(pos - p.Position), tlen);
						long posbk = pos;
						BinaryPage pbk = p;
						undo.Push(delegate() {
							BinaryPage pbk2 = pbk;
							byte[] databk2 = databk;
							pbk2.Buffer.InsertRange((int)(posbk - pbk2.Position), databk2);
						});
						flg = true;
					}
					len -= tlen;
					pos += tlen;
					deleted += tlen;

					if (deleted >= slen) {
						if (deleted != tlen) {
							long posbk = p.Position;
							p.Position = spos;
							undo.Push(delegate() {
								long posbk2 = posbk;
								p.Position = posbk2;
							});
						}
						break;
					}
				}
			}
			while (ie.MoveNext()) {
				p = ie.Current;
				p.Position -= slen;
				undo.Push(delegate() {
					p.Position += slen;
				});
				flg = true;
			}
			if (flg == false) {
				pages.AddLast(new BinaryPage(0, 0));
				undo.Push(delegate() {
					pages.RemoveLast();
				});
				long lengthbk = length;
				length = 0;
				undo.Push(delegate() {
					length = lengthbk;
				});
				undoBuffer.Push(undo);
				return;
			}
			length -= slen;
			undo.Push(delegate() {
				length += slen;
			});
			undoBuffer.Push(undo);
		}

		public void OverWrite(byte data, long pos)
		{
			Stack<UndoDelegate> undo = new Stack<UndoDelegate>();
			RunBeforeStackUndo(undo);
			BinaryPage p = GetPageByPos(pos);
			MakeBuffer(p);
			if (pos < length) {
				byte databk = p.Buffer[(int)(pos - p.Position)];
				p.Buffer.RemoveAt((int)(pos - p.Position));
				undo.Push(delegate() {
					p.Buffer.Insert((int)(pos - p.Position), databk);
				});
				p.Buffer.Insert((int)(pos - p.Position), data);
				undo.Push(delegate() {
					p.Buffer.RemoveAt((int)(pos - p.Position));
				});
			} else {
				p.Buffer.Add(data);
				undo.Push(delegate() {
					p.Buffer.RemoveAt(p.Buffer.Count - 1);
				});
				length++;
				undo.Push(delegate() {
					length--;
				});
			}
			undoBuffer.Push(undo);
		}

		public void OverWriteRange(byte[] data, long pos, int len)
		{
			Stack<UndoDelegate> undo = new Stack<UndoDelegate>();
			RunBeforeStackUndo(undo);
			int tlen = 0, overwrited = 0, slen = len;
			long spos = pos;
			foreach (BinaryPage p in pages) {
				if (!p.Removed && p.Position >= pos - PAGE_LEN) {
					if (MakeBuffer(p)) {
						undo.Push(delegate() {
							RemoveBuffer(p);
						});
					}
					tlen = (int)Math.Min(p.Length - (pos - p.Position), len);
					if (tlen == p.Length) {
						byte[] databk = p.Buffer.ToArray();
						p.Buffer.Clear();
						undo.Push(delegate() {
							p.Buffer.AddRange(databk);
						});
						byte[] buf = new byte[tlen];
						Array.Copy(data, overwrited, buf, 0, tlen);
						p.Buffer.InsertRange(0, buf);
						undo.Push(delegate() {
							p.Buffer.RemoveRange(0, buf.Length);
						});
					} else {
						byte[] databk = new byte[tlen];
						p.Buffer.CopyTo((int)(pos - p.Position), databk, 0, tlen);
						byte[] buf = new byte[tlen];
						Array.Copy(data, overwrited, buf, 0, tlen);
						p.Buffer.InsertRange((int)(pos - p.Position), buf);
						undo.Push(delegate() {
							p.Buffer.RemoveRange((int)(pos - p.Position), buf.Length);
						});
						p.Buffer.RemoveRange((int)(pos - p.Position) + tlen, tlen);
						undo.Push(delegate() {
							p.Buffer.InsertRange((int)(pos - p.Position), databk);
						});
					}
					len -= tlen;
					pos += tlen;
					overwrited += tlen;

					if (overwrited >= slen) {
						break;
					}
				}
			}
			if (overwrited < slen) {
				tlen = slen - overwrited;
				byte[] buf = new byte[tlen];
				Array.Copy(data, overwrited, buf, 0, tlen);
				int idxbk = pages.Last.Value.Length;
				pages.Last.Value.Buffer.AddRange(buf);
				undo.Push(delegate() {
					pages.Last.Value.Buffer.RemoveRange(idxbk, buf.Length);
				});
			}
			length = Math.Max(spos + slen, length);
			undoBuffer.Push(undo);
		}

		public void Save(Stream s)
		{
			if (!s.CanWrite) {
				throw new ArgumentException("sには書き込み可能なストリームのみ指定できます。");
			}
			s.SetLength(length);
			foreach (BinaryPage p in pages) {
				if (p.Removed) {
					continue;
				}
				byte[] buf = new byte[p.Length];
				if (p.Modified) {
					p.Buffer.CopyTo(buf);
				} else {
					stream.Seek(p.SourcePosition, SeekOrigin.Begin);
					stream.Read(buf, 0, p.Length);
				}
				s.Write(buf, 0, p.Length);
			}
			s.Close();
		}

		public bool Undo()
		{
			if (undoBuffer.Count > 0) {
				Stack<UndoDelegate> undo = undoBuffer.Pop();
				while (undo.Count > 0) {
					undo.Pop()();
				}
				return true;
			}
			return false;
		}

		#region IDisposable メンバ

		public void Dispose()
		{
			stream.Dispose();
		}

		#endregion
	}
}
