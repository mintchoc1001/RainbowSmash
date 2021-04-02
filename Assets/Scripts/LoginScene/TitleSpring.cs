using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSpring : MonoBehaviour
{
    public GameObject startPoints;
    [SerializeField]
    Transform[] movePoint;

    public int step = 1; // 3 31 변경
    float speed = 0.5f;

    bool isStepPlus = false;

    public GameObject Logn;

    private void Start()
    {
        movePoint = startPoints.GetComponentsInChildren<Transform>();
        Logn.gameObject.SetActive(false);
    }
    void Update()
    {
        UIMove_n_Bounce();
        print(step);
    }

    void UIMove_n_Bounce()
    {
        if (Input.GetMouseButtonDown(0)) // 3 31 변경
        {
            step = 6;
            transform.position = movePoint[step].position;

            Logn.gameObject.SetActive(true);
            gameObject.GetComponent<TitleSpring>().enabled = false;
        }

        if ((step == 1 || step == 3) && Vector3.Distance(transform.position, movePoint[step].position) >= 0.1f)
        {
            transform.position += (Vector3.down) * 2f * speed;
            //if (step == 3) speed += 0.00032f;
            if (Vector3.Distance(transform.position, movePoint[step].position) <= 0.2f)
            {
                //if (step == 1) speed *= 0.06f;
                step++;
            }
        }


        if ((step == 2 || step == 4) && Vector3.Distance(transform.position, movePoint[step].position) >= 0.1f)
        {
            transform.position += (Vector3.up) * 2f * speed;
            //transform.position = Vector3.Lerp(transform.position, movePoint[step].position, 0.15f);
            if (Vector3.Distance(transform.position, movePoint[step].position) <= 0.1f)
            {
                //speed *= 0.0001f;
                step++;
            }
        }



        if (step == 5)
        {
            transform.position += (Vector3.down) * 1.5f * speed;

            if (Vector3.Distance(transform.position, movePoint[step].position) <= 1f)
            {

                transform.position = movePoint[step].position;

                if (!isStepPlus)
                {
                    isStepPlus = true;
                    // 몇초뒤에 위로 이동
                    Invoke("NextStep", 2f);
                }

            }
        }

        if (step == 6)
        {
            transform.position += (Vector3.up) * 2f * speed;
            if (Vector3.Distance(transform.position, movePoint[step].position) <= 1f)
            {
                transform.position = movePoint[step].position;

                // 이후에 게임시작창이 나오게 함
                Logn.gameObject.SetActive(true);
                gameObject.GetComponent<TitleSpring>().enabled = false;

            }
        }

    }

    void NextStep()
    {
        step += 1;
    }

}
