using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MathQuizManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text questionNumberText;

    [Header("Game Settings")]
    public int totalQuestions = 10;
    public int maxNumber = 20;

    private int currentQuestionIndex = 0;
    private int correctAnswers = 0;
    private int correctAnswer;
    private List<Question> questions = new List<Question>();
    private Animator characterAnimator;
    void Start()
    {
        GameObject characterSwitcher = GameObject.Find("SceneCharacterSwitcher");

        if (characterSwitcher == null) 
            characterAnimator = null;
        else
        {
            foreach (Transform child in characterSwitcher.transform)
            {
                if (child.gameObject.activeInHierarchy)
                    characterAnimator = child.GetComponent<Animator>();
            }
        }

        if (characterAnimator == null)
            Debug.LogError("❌ No active character Animator found!");

        GenerateQuestions();
        DisplayQuestion();
    }

    private class Question
    {
        public string questionText;
        public int correctAnswer;
    }

    void GenerateQuestions()
    {
        questions.Clear();

        for (int i = 0; i < totalQuestions; i++)
        {
            int num1 = Random.Range(1, maxNumber + 1);
            int num2 = Random.Range(1, maxNumber + 1);

            int operation = Random.Range(0, 3);

            Question q = new Question();

            switch (operation)
            {
                case 0: // Addition
                    q.questionText = $"{num1} + {num2} = ?";
                    q.correctAnswer = num1 + num2;
                    break;

                case 1: // Subtraction (keep result positive)
                    if (num1 < num2)
                    {
                        int temp = num1;
                        num1 = num2;
                        num2 = temp;
                    }
                    q.questionText = $"{num1} - {num2} = ?";
                    q.correctAnswer = num1 - num2;
                    break;

                case 2: // Multiplication (smaller numbers)
                    num1 = Random.Range(2, 11);
                    num2 = Random.Range(2, 11);
                    q.questionText = $"{num1} × {num2} = ?";
                    q.correctAnswer = num1 * num2;
                    break;
            }

            questions.Add(q);
        }
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex >= totalQuestions)
        {
            ShowResults();
            return;
        }

        Question q = questions[currentQuestionIndex];
        questionText.text = q.questionText;
        correctAnswer = q.correctAnswer;

        questionNumberText.text = $"Question {currentQuestionIndex + 1}/{totalQuestions}";

        List<int> options = GenerateAnswerOptions(correctAnswer);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = options[i];
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = answer.ToString();

            answerButtons[i].onClick.RemoveAllListeners();

            int capturedAnswer = answer;
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(capturedAnswer));

            answerButtons[i].interactable = true;
        }
    }

    List<int> GenerateAnswerOptions(int correct)
    {
        List<int> options = new List<int> { correct };

        while (options.Count < 4)
        {
            // Create wrong answer near the correct one
            int offset = Random.Range(-10, 11);
            if (offset == 0) offset = Random.Range(1, 6); // Avoid generating correct answer

            int wrongAnswer = correct + offset;

            // make sure it's positive and not duplicate
            if (wrongAnswer > 0 && !options.Contains(wrongAnswer))
            {
                options.Add(wrongAnswer);
            }
        }

        ShuffleList(options);

        return options;
    }

    void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void OnAnswerSelected(int selectedAnswer)
    {
        foreach (Button btn in answerButtons)
            btn.interactable = false;

        if (selectedAnswer == correctAnswer)
        {
            correctAnswers++;
            characterAnimator.SetTrigger("Happy");
            if (SettingsManager.Instance != null)
                SettingsManager.Instance.PlayCorrect();
        }
        else
        {
            characterAnimator.SetTrigger("Sad");
            if (SettingsManager.Instance != null)
                SettingsManager.Instance.PlayWrong();
        }

        Invoke(nameof(NextQuestion), 0.5f);
    }

    void NextQuestion()
    {
        currentQuestionIndex++;
        DisplayQuestion();
    }

    void ShowResults()
    {
        float percentage = (float)correctAnswers / totalQuestions * 100f;

        int starsEarned = 0;
        if (percentage >= 90) starsEarned = 5;
        else if (percentage >= 80) starsEarned = 4;
        else if (percentage >= 60) starsEarned = 3;
        else if (percentage >= 40) starsEarned = 2;
        else if (percentage >= 20) starsEarned = 1;

        string message = $"You got {correctAnswers} out of {totalQuestions} correct!";

        StarPopupManager.Instance.ShowStars(starsEarned, message);
    }

    public void RestartQuiz()
    {
        currentQuestionIndex = 0;
        correctAnswers = 0;
        StarPopupManager.Instance.PlayAgain();
        GenerateQuestions();
        DisplayQuestion();
    }

    public void BackToMenu()
    {
        StarPopupManager.Instance.GoToMainMenu();
    }
}