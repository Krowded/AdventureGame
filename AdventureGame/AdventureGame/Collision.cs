using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventureGame
{
    class Collision
    {
        public bool CollisionCheck(List<InteractiveObject> thingList, Player player)
        {
            foreach (InteractiveObject thing in thingList)
            {
                if (thing.Collidable && CollidesWithPlayer(thing, player))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CollidesWithPlayer(InteractiveObject thing, Player player)
        {
            return (CollidesWithPlayerX(thing, player) && CollidesWithPlayerY(thing, player));
        }

        private bool CollidesWithPlayerX(InteractiveObject thing, Player player)
        {
            return ((player.Position.X + player.Width * player.Scale / 2 > thing.Position.X) && (player.Position.X - player.Width * player.Scale / 2 < (thing.Position.X + thing.Texture.Width * thing.Scale)));
        }

        private bool CollidesWithPlayerY(InteractiveObject thing, Player player)
        {
            return ((player.Position.Y + player.Height * player.Scale / 2 > thing.Position.Y) && (player.Position.Y - player.Height * player.Scale / 2 < (thing.Position.Y + thing.Texture.Height * thing.Scale)));
        }
    }
}
