using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labels
{
    public static class Tags
    {
        public const string ENEMY = "Enemy";
        public const string PLAYER = "Player";
        public const string PROJECTILE = "Projectile";
        public const string PICKUP = "Pickup";
    }

    public static class PhysicsLayers
    {

    }

    public static class Inputs
    {
        public const string HORIZONTAL_AXIS = "Horizontal";
        public const string VERTICAL_AXIS = "Vertical";
        public const string SHOOT = "Fire";
    }

    public static class AnimProperties
    {
        public const string DAMAGE_TRIG = "Damage";
        public const string DEATH_TRIG = "Death";
        public const string CHARGING = "Charging";
        public const string MOVE_LEFT = "Move_Left";
        public const string MOVE_RIGHT = "Move_Right";
        public const string MOVE_UP = "Move_Up";
        public const string MOVE_DOWN = "Move_Down";
    }

    public static class UIButtonAnimProperties
    {
        public const string NORMAL = "Normal";
        public const string HIGHLIGHTED = "Highlighted";
        public const string PRESSED = "Pressed";
        public const string SELECTED = "Selected";
        public const string DISABLED = "Disabled";
    }

    public static class UITextAnimProperties
    {
        public const string SELECTED_TRIG = "Selected";
    }
}
