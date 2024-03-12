using System;

namespace DuckGame.Halloween
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    class CustomHat : Attribute
    {
        public string Name;
        public string TexturePath;
        public CustomHat(string name, string hatTexture)
        {
            Name = name;
            TexturePath = Mod.GetPath<Halloween>(hatTexture);
        }
    }
}