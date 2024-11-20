using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Scripts.Modules.Resolutions
{
    public class ScreenService : IScreenService
    {
        private List<Resolution> resolutions;
        private int resolutionIndex;
        
        public int ResolutionIndex
        {
            get => resolutionIndex;
            set
            {
                resolutionIndex = value;
                SetResolutionByIndex(resolutionIndex);
            }
        }

        public bool IsFullScreen
        {
            get => Screen.fullScreen;
            set => Screen.fullScreen = value;
        }

        private List<Resolution> Resolutions
        {
            get
            {
                if (resolutions == null || resolutions.Count == 0)
                {
                    return GetResolutions();
                }

                return resolutions;
            }
        }

        private void SetResolutionByIndex(int index)
        {
            var resolution = Resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
        
        private void SetResolution(int width, int height)
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
            ResolutionIndex = GetIdFromResolution(width, height);
        }
        
        public List<Resolution> GetResolutions()
        {
            var result = Screen.resolutions.ToList();
            result.Reverse();
            return result;
        }

        public List<string> GetStringResolutions()
        {
            return Resolutions
                .Select(res => $"{res.width.ToString()}x{res.height.ToString()}")
                .ToList();
        }

        private int GetIdFromResolution(int width, int height)
        {
            return Resolutions
                .FindIndex(x => 
                    x.width == width &&
                    x.height == height
                    );
        }
    }
}