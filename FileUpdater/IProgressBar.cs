using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpdater
{
    interface IProgressBar
    {
        void Inicialize(int filesCount);

        void Increment();
    }
}
