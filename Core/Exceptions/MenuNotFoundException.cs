namespace KitchenPC
{
    using System;

    public class MenuNotFoundException : KPCException
    {
        public MenuNotFoundException()
        {
        }

        public MenuNotFoundException(Guid menuId)
        {
            this.MenuId = menuId;
        }

        public Guid? MenuId { get; private set; }
    }
}