using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class JigsawData
{
    public string word;
    public Texture2D image;
}
public class GameManager : MonoBehaviour
{
    [SerializeField] JigsawData[] jigsawData;
    private DraggableObject[] puzzlePieces;
    private int currentPuzzleIndex = 0;

    private void OnEnable()
    {
        DraggableObject.onPuzzlePieceSnapped += CheckIfPuzzleIsComplete;
    }

    private void OnDisable()
    {
        DraggableObject.onPuzzlePieceSnapped -= CheckIfPuzzleIsComplete;
    }

    private void Start()
    {
        puzzlePieces = FindObjectsOfType<DraggableObject>();
        BuildPuzzle();
    }

    void BuildPuzzle()
    {
        // put puzzle pieces in random positions
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            puzzlePieces[i].transform.position = new Vector3(Random.Range(-9, 9), Random.Range(-4, 0), 0);
            puzzlePieces[i].isSnapped = false;
        }

        // set tmpro text to letter of the word
        for (int i = 0; i < jigsawData[currentPuzzleIndex].word.Length; i++)
        {
            var text = puzzlePieces[i].gameObject.GetComponentInChildren<TextMeshPro>();
            text.text = jigsawData[currentPuzzleIndex].word[i].ToString();
        }

        // set image to the image of the word
        var image = GameObject.Find("Image");
        var sprite = Sprite.Create(jigsawData[currentPuzzleIndex].image, new Rect(0, 0, jigsawData[currentPuzzleIndex].image.width, jigsawData[currentPuzzleIndex].image.height), new Vector2(0.5f, 0.5f));
        image.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    void CheckIfPuzzleIsComplete()
    {
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            if (!puzzlePieces[i].isSnapped)
            {
                return;
            }
        }

        LoadNextChallenge();
    }

    void LoadNextChallenge()
    {
        if (currentPuzzleIndex < jigsawData.Length - 1)
        {
            currentPuzzleIndex++;
            BuildPuzzle();
        }
        else
        {
            GoToNextScene();
        }
    }
    public void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
