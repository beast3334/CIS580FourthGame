﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
namespace MonoGameWindowsStarter
{
    class BulletModel
    {
        public Texture2D texture;

        public BulletModel(ContentManager content)
        {
            texture = content.Load<Texture2D>("bullet");
        }
    }
}
