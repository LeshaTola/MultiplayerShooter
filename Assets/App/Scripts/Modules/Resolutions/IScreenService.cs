using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Modules.Resolutions
{
    public interface IScreenService
    {
        List<Resolution> GetResolutions();
        List<string> GetStringResolutions();

        int ResolutionIndex { get; set; }
        bool IsFullScreen { get; set; }
    }
}