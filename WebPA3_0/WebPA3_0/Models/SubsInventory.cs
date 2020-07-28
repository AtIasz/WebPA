using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPA.Models
{
    public class SubsInventory
    {
        public int _item_id;
        public int _user_id;
        public SubsInventory()
        {

        }
        public SubsInventory(int item_id, int user_id)
        {
            this._item_id = item_id;
            this._user_id = user_id;
        }
    }
}
