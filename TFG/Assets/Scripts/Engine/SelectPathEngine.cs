using UnityEngine;
using System.Collections;

public class SelectPathEngine : MonoBehaviour {

    private bool isPath = false;

    public void setSelection(bool path)
    {
        this.isPath = path;
    }

    public bool isSelection()
    {
        return isPath;
    }
}
