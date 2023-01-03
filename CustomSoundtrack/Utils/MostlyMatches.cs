using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.SceneManagement;

namespace CustomSoundtrack.Utils
{
    static partial class Extensions
    {
        public static bool MostlyMatches(this Scene scene, string[] scenes) => scenes.Any(e => scene.name.Contains(e));
    }
}
