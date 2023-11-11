using Niantic.Lightship.AR.Semantics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class SemanticQuerying : MonoBehaviour
{
    public ARCameraManager _cameraMan;
    public ARSemanticSegmentationManager _semanticMan;

    public TMP_Text _text;

    private string _channel = "ground";
    void OnEnable()
    {
        _cameraMan.frameReceived += OnCameraFrameUpdate;
    }

    private void OnDisable()
    {
        _cameraMan.frameReceived -= OnCameraFrameUpdate;
    }

    private void OnCameraFrameUpdate(ARCameraFrameEventArgs args)
    {
        if (!_semanticMan.subsystem.running)
        {
            return;
        }

        //get the semantic texture
        Matrix4x4 mat = Matrix4x4.identity;
        var texture = _semanticMan.GetSemanticChannelTexture(_channel, out mat);
    }

    private float _timer = 0.0f;
    void Update()
    {
        if (!_semanticMan.subsystem.running)
        {
            return;
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Input.mousePosition;
#else
        if (Input.touches.Length > 0)
            {
            var pos = Input.touches[0].position;
#endif

            if (pos.x > 0 && pos.x < Screen.width)
                if (pos.y > 0 && pos.y < Screen.height)
                {
                    _timer += Time.deltaTime;
                    if (_timer > 0.05f)
                    {
                        var list = _semanticMan.GetChannelNamesAt((int)pos.x, (int)pos.y);

                        if (list.Count > 0)
                        {
                            _channel = list[0];
                            _text.text = _channel;
                        }
                        else
                        {
                            _text.text = "?";
                        }

                        _timer = 0.0f;
                    }
                }
        }
    }
}