using System.Collections.Generic;
using System.Threading;

namespace SkypeIntegration
{
	public class ConcurrentGroupQueue {
		private string _lastKey = null;
		private readonly List<KeyValuePair<string, string>> _queue = new List<KeyValuePair<string, string>>();
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

		public void Add(string key, string value) {
			_semaphore.Wait();
			_queue.Add(new KeyValuePair<string, string>(key, value));
			_semaphore.Release();
		}

		/// <summary>
		/// Get message for send with high priority (for minimize count of switches between recipients)
		/// </summary>
		/// <returns></returns>
		public KeyValuePair<string, string>? GetNext() {
			_semaphore.Wait();
			if (_queue == null || _queue.Count == 0){
				_semaphore.Release();
				return null;
			}

			var pairIndex = _queue.FindIndex(p => p.Key == _lastKey);

			if (pairIndex < 0) {
				pairIndex = 0;
			}

			var nextPairForFill = _queue[pairIndex];
			_queue.RemoveAt(pairIndex);
			_lastKey = nextPairForFill.Key;
			_semaphore.Release();
			return nextPairForFill;
		}
	}
}