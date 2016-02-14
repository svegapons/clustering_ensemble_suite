using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClusterEnsemble.DataStructures
{   

    public class HeapArray<T> where T: IRank
    {
        protected T[] o;
        protected int count;

        public HeapArray(int capacity)
        {
            o = new T[capacity + 1];
            count = 0;
        }

        public void BuildHeap(T[] a)
        {
            o = a;
            for (int i = a.Length / 2 - 1; i >= 1; i--)
                BubblingDowm(i);

        }
        public T First
        {
            get
            {
                return o[1];
            }
        }
        public T[] ToArray
        {
            get
            {
                return o;
            }
        }
        public int Count
        {
            get
            {
                return count;
            }
        }
        protected T Root(int pos)
        {
            if (pos % 2 == 0 && pos > 1)
                return o[pos / 2];
            return default(T);
        }
        protected T RightChild(int pos)
        {
            if (pos >= 0 && 2 * pos + 1 < o.Length)
                return o[2 * pos + 1];
            return default(T);
        }
        protected T LeftChild(int pos)
        {
            if (pos >= 0 && 2 * pos < o.Length)
                return o[2 * pos];
            return default(T);
        }
        protected virtual void BubblingDowm(int pos)
        {
            if (pos > 0 && pos < count)
            {
                if ((2 * pos < count && 2 * pos + 1 < count) && (o[pos].Rank > RightChild(pos).Rank || o[pos].Rank > LeftChild(pos).Rank))
                {
                    if (RightChild(pos).Rank < LeftChild(pos).Rank)
                    {
                        T a = o[pos];
                        o[pos] = RightChild(pos);
                        o[2 * pos + 1] = a;
                        BubblingDowm(2 * pos + 1);
                    }
                    else
                    {
                        T a = o[pos];
                        o[pos] = LeftChild(pos);
                        o[2 * pos] = a;
                        BubblingDowm(2 * pos);
                    }
                }
                else if (2 * pos >= count && 2 * pos + 1 >= count)
                    return;
                else if (2 * pos >= count && o[pos].Rank > RightChild(pos).Rank)
                {
                    T a = o[pos];
                    o[pos] = RightChild(pos);
                    o[2 * pos + 1] = a;
                    BubblingDowm(2 * pos + 1);
                }
                else if (2 * pos + 1 >= count && o[pos].Rank > LeftChild(pos).Rank)
                {
                    T a = o[pos];
                    o[pos] = LeftChild(pos);
                    o[2 * pos] = a;
                    BubblingDowm(2 * pos);
                }

            }
            return;
        }
        protected virtual void BubblingUp(int pos)
        {
            if (pos > 1 && o[pos].Rank < o[pos / 2].Rank)
            {
                T temp = o[pos];
                o[pos] = o[pos / 2];
                o[pos / 2] = temp;
                BubblingUp(pos / 2);
            }
            return;
        }
        public void RemoveFirst()
        {
            if (count > 0)
            {
                o[1] = o[count];
                o[count] = default(T);
                count--;
                BubblingDowm(1);
            }
            return;

        }
        public void Add(T a)
        {
            o[count + 1] = a;
            count++;
            BubblingUp(count);
        }
    }

    public class HeapArrayMax<T>:HeapArray<T> where T : IRank
    { 
        public HeapArrayMax(int count):base(count){}
      
        protected override void BubblingDowm(int pos)
        {
            if (pos > 0 && pos < count)
            {
                if ((2 * pos < count && 2 * pos + 1 < count) && (o[pos].Rank < RightChild(pos).Rank || o[pos].Rank < LeftChild(pos).Rank))
                {
                    if (RightChild(pos).Rank > LeftChild(pos).Rank)
                    {
                        T a = o[pos];
                        o[pos] = RightChild(pos);
                        o[2 * pos + 1] = a;
                        BubblingDowm(2 * pos + 1);
                    }
                    else
                    {
                        T a = o[pos];
                        o[pos] = LeftChild(pos);
                        o[2 * pos] = a;
                        BubblingDowm(2 * pos);
                    }
                }
                else if (2 * pos >= count && 2 * pos + 1 >= count)
                    return;
                else if (2 * pos >= count && o[pos].Rank < RightChild(pos).Rank)
                {
                    T a = o[pos];
                    o[pos] = RightChild(pos);
                    o[2 * pos + 1] = a;
                    BubblingDowm(2 * pos + 1);
                }
                else if (2 * pos + 1 >= count && o[pos].Rank < LeftChild(pos).Rank)
                {
                    T a = o[pos];
                    o[pos] = LeftChild(pos);
                    o[2 * pos] = a;
                    BubblingDowm(2 * pos);
                }

            }
            return;
        }
        protected override void BubblingUp(int pos)
        {
            if (pos > 1 && o[pos].Rank > o[pos / 2].Rank)
            {
                T temp = o[pos];
                o[pos] = o[pos / 2];
                o[pos / 2] = temp;
                BubblingUp(pos / 2);
            }
            return;
        }
        
    }

    public interface IRank
    {
        double Rank {get;set;}
    }

    public class Container : IRank
    {
        public int Cluster { get; set; }
        public int Name { get; set; }

        public double Rank { get; set; }
    }

    public class Pair : IRank
    {
        public double Rank { get; set; }
        public Structuring P1 { get; set; }
        public Structuring P2 { get; set; }

        public Cluster C1 { get; set; }
        public Cluster C2 { get; set; }
    }
}
