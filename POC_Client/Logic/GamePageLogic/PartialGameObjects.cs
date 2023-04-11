using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POC_Client.Objects;

namespace POC_Client.Logic
{
    public abstract partial class Game
    {
        public event EventHandler<ScreenObject> AddScreenObject;



        protected virtual void OnAddScreenObject(ScreenObject i_ScreenObject)
        {
            AddScreenObject.Invoke(this, i_ScreenObject);
        }
    }
}
