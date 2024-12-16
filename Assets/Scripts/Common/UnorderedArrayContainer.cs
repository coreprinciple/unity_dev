using System;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class UnorderedArrayContainer<T> : IEnumerable<T>
    {
        private struct SearchData
        {
            public int x, y;
        }

        public T this[int i] => dataArray[i];

        public T[] dataArray = new T[10];
        public int Count { get; private set; }

        private readonly Queue<SearchData> _searchTempQueue = new Queue<SearchData>();

        public UnorderedArrayContainer(int arraySize)
        {
            dataArray = new T[arraySize];
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return dataArray[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return dataArray[i];
        }

        public void Add(T item)
        {
            if (Count == dataArray.Length)
            {
                int length = dataArray.Length;
                Array.Resize(ref dataArray, length * 2);

                dataArray[length] = item;
            }
            else
                dataArray[Count] = item;

            Count++;
        }

        public void Set(int index, T item) => dataArray[index] = item;

        public bool Contains(T item)
        {
            for (int u = 0; u < Count; u++)
            {
                if (dataArray[u].Equals(item) == false)
                    continue;
                return true;
            }
            return false;
        }

        public bool Remove(T item)
        {
            for (int u = 0; u < Count; u++)
            {
                if (dataArray[u].Equals(item) == false)
                    continue;

                Count--;
                dataArray[u] = dataArray[Count];
                dataArray[Count] = default;
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            Count--;
            dataArray[index] = dataArray[Count];
            dataArray[Count] = default;
        }

        public int BinarySearch(T value)
        {
            if (Count == 0)
                return -1;

            int found = BinarySearch(0, Count - 1, value);
            if (found < 0)
            {
                while (_searchTempQueue.Count > 0)
                {
                    SearchData range = _searchTempQueue.Dequeue();
                    found = BinarySearch(range.x, range.y, value);
                    if (found >= 0)
                        break;
                }
            }
            return found;
        }

        private int BinarySearch(int start, int end, T value)
        {
            if (end == start)
            {
                if (IsEquals(dataArray[start], value))
                    return start;
                return -1;
            }

            int length = end - start;
            if (length == 1)
            {
                if (IsEquals(dataArray[start], value))
                    return start;
                if (IsEquals(dataArray[end], value))
                    return end;
                return -1;
            }

            int middle = (end + start) / 2;
            if (IsEquals(dataArray[middle], value))
                return middle;

            SearchData range0 = new SearchData();
            range0.x = start;
            range0.y = middle - 1;

            SearchData range1 = new SearchData();
            range1.x = middle + 1;
            range1.y = end;

            _searchTempQueue.Enqueue(range0);
            _searchTempQueue.Enqueue(range1);

            return -1;
        }

        protected virtual bool IsEquals(T pivot, T value)
        {
            throw new NotImplementedException("Should Implement Object Equals Method");
        }

        public void Dispose()
        {
            for (int u = 0; u < dataArray.Length; u++)
            {
                if (dataArray[u] == null)
                    break;

                dataArray[u] = default;
            }
            Count = 0;
        }
    }
}