#region imports

using JetBrains.Annotations;
using UnityEngine;

#endregion

namespace Shkoda.RecognizeMe.Core
{
    public class GameProperties : MonoBehaviour
    {
        [EditorAssigned] public int ColumnNumber = 8;
        [EditorAssigned] public int RowNumber = 8;
    }
}