using System;

namespace KitchenPC.Menus
{
    public class MenuResult
    {
        public bool IsMenuCreated { get; set; }

        public bool IsMenuUpdated { get; set; }

        public Guid? NewMenuId { get; set; }
    }
}