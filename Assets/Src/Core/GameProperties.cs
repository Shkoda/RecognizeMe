using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Shkoda.RecognizeMe.Core
{
    public class GameProperties : MonoBehaviour
    {
        [EditorAssigned] public int RowNumber = 8;
        [EditorAssigned] public int ColumnNumber = 8;
    }
}