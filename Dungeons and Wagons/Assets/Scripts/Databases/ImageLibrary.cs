using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to store all images for the user interface.
/// Thought Process : Wanted to cache images for easy access by User interface objects
/// I didn't want images to be cached on each item object but rather have a library of preloaded 
/// images to search from that matches the name
/// </summary>

public class ImageLibrary : MonoBehaviour {
    Dictionary<string, Sprite> textureLibrary;
    [SerializeField]
    Object[] textures;
    Dictionary<string, Sprite> emotionLibrary;
    [SerializeField]
    Object[] emotionTextures;

    public void InitializeImageLibrary() {
        textureLibrary = new Dictionary<string, Sprite>();
        emotionLibrary = new Dictionary<string, Sprite>();
        LoadTextures();
    }
    void LoadTextures() {
        textures = Resources.LoadAll("UserInterface", typeof(Sprite));
        foreach (Sprite t in textures) {
            textureLibrary.Add(t.name, t as Sprite);
        }
        emotionTextures = Resources.LoadAll("UserInterface/Emotes", typeof(Sprite));
        foreach(Sprite t in emotionTextures) {
            emotionLibrary.Add(t.name, t as Sprite);
        }
    }
    public Sprite GetSpriteReference(string name) {
        return textureLibrary[name];
    }
    public Dictionary<string, Sprite> GetEmotionLibrary() {
        if (emotionLibrary.Count > 0 && emotionLibrary != null) {
            return emotionLibrary;
        } else
            return null;
    }
    public Sprite GetEmotionReference(string name) {
        return emotionLibrary[name];
    }
}
