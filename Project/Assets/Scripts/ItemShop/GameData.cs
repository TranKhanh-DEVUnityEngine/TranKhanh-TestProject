using System;
using Do.Scripts.Tools.Other;

public static class GameData
{

    public static class JsonHelper
    {
        public static T FromJsonBase<T>(string json)
        {
            WrapperBase<T> wrapper = UnityEngine.JsonUtility.FromJson<WrapperBase<T>>(json);
            return wrapper.Items;
        }
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return UnityEngine.JsonUtility.ToJson(wrapper);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }

        [Serializable]
        private class WrapperBase<T>
        {
            public T Items;
        }
    }

}