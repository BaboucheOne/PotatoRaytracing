using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace PotatoRaytracing
{
    public abstract class PotatoEntity
    {
        public Vector3 Position = new Vector3();
        public Quaternion Rotation = new Quaternion();

        public PotatoEntity(Vector3 position)
        {
            Position = position;
        }

        public PotatoEntity(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public abstract void Serialize(ref Dictionary<string, string> serialRead);
    }

    public class PotatoEntites : IEnumerable
    {
        private PotatoEntity[] potatoEntity;
        public PotatoEntites(PotatoEntity[] pArray)
        {
            potatoEntity = new PotatoEntity[pArray.Length];

            for (int i = 0; i < pArray.Length; i++)
            {
                potatoEntity[i] = pArray[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public PotatoEntityEnum GetEnumerator()
        {
            return new PotatoEntityEnum(potatoEntity);
        }
    }

    public class PotatoEntityEnum : IEnumerator
    {
        public PotatoEntity[] entity;

        int position = -1;

        public PotatoEntityEnum(PotatoEntity[] list)
        {
            entity = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < entity.Length);
        }
        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        public PotatoEntity Current
        {
            get
            {
                try
                {
                    return entity[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}