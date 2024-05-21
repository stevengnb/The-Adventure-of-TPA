using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionControl : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] allRes;
    private List<Resolution> filtered;

    private float currRefreshRate;
    private int currResolutionIndex;

    public void Start() {
        allRes = Screen.resolutions;
        filtered = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currRefreshRate = Screen.currentResolution.refreshRate;

        for(int p = 0; p < allRes.Length; p++) {
            if(allRes[p].refreshRate == currRefreshRate) {
                filtered.Add(allRes[p]);
            }
        }

        List<string> options = new List<string>();
        for(int p = 0; p < filtered.Count; p++) {
            string resFromSystem = filtered[p].width + "x" + filtered[p].height + " " + filtered[p].refreshRate + " Hz";
            options.Add(resFromSystem);

            if(filtered[p].width == Screen.width && filtered[p].height == Screen.height) {
                currResolutionIndex = p;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resIndex) {
        Resolution res = filtered[resIndex];
        if(res.width >= 800 && res.height >= 600) {
            Screen.SetResolution(res.width, res.height, true); 
        }
    }
}
