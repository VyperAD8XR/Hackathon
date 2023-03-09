using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using Random = UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public QuestionData quizQuestions;
    [HideInInspector]
    public static GameManager Instance;


    public TextMeshProUGUI screenQuestion;
    public TextMeshProUGUI scoreInccorect;
    public TextMeshProUGUI scoreCorrect;
    public TextMeshProUGUI questionCount;

    private string _quizEndpoint;
    private int _correctAnswers = 0;
    private int _incorrectAnswers = 0;
    private int _totalQuestions = 20;


    private void Awake()
    {
        Instance = this;
        //Currently all categories are used, the difficulty is 'easy', and the total questions IS 20. All can be configured via UI in the future wanted to utilize the limted time for other aspects of the project
        _quizEndpoint = "https://the-trivia-api.com/api/questions?categories=food_and_drink,general_knowledge,geography,history,music,science,society_and_culture,sport_and_leisure,film_and_tv,arts_and_literature&limit=" + totalQuestions + "& difficulty=easy";

    }
    // Start is called before the first frame update
    void Start()
    {
        LoadQuestions();
    }

    public string LoadQuestions()
    {

        var request = WebRequest.Create(_quizEndpoint) as HttpWebRequest;

        request.KeepAlive = true;
        request.Method = "GET";
        request.ContentType = "application/json; charset=utf-8";

        string responseContent = null;

        try
        {

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    responseContent = "{\"Questions\":" + reader.ReadToEnd() + "}";
                }
            }
            quizQuestions = JsonConvert.DeserializeObject<QuestionData>(responseContent);

            return responseContent;
        }
        catch (WebException wex)
        {
            using (var Errreader = new StreamReader(wex.Response.GetResponseStream()))
            {
                string responseErr = Errreader.ReadToEnd();
                return responseErr;
            }

        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public void UpdateScore(bool _isCorrect)
    {
        _correctAnswers = (_isCorrect ? _correctAnswers++ : _correctAnswers);
        _incorrectAnswers = (_isCorrect ? _incorrectAnswers++ : _incorrectAnswers);

        scoreCorrect.text = _correctAnswers.ToString();
        scoreInccorect.text = _incorrectAnswers.ToString();
        questionCount.text = (_correctAnswers + _incorrectAnswers).ToString() + "/" + _totalQuestions;
    }
}