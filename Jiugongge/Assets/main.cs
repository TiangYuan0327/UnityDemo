using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class main : MonoBehaviour
{
    public TextMeshPro mytext;
    public Texture2D img, mainImg;
    public Texture2D[] imgpool;

    public GameObject cube, objcopy;
    public GameObject[,] gameObjpool;
    public int size = 3;
    public GameObject exit;

    Stack<int[]> record = new Stack<int[]>(); 



    // Start is called before the first frame update
    public int[] vect;
    int ti = 0, tj = 0;
    void Start()
    {

        imgpool = new Texture2D[size * size];
        int w, h;
        w = (int)Mathf.Floor((float)img.width/(float)size);
        h = (int)Mathf.Floor((float)img.height / (float)size);

        //print("x" + (5 % size).ToString());
       // print("y" + (Mathf.Floor(8 / size)).ToString());

        for (int i = 0; i < size * size; i++)
        {
            imgpool[i] = new Texture2D(w, h);
            int ix = (size - 1) - ( i % size);
            int iy = (int)Mathf.Floor(i / size); 
            for(int x = 0; x < w; x++)
            {
                for(int y = 0; y < h; y++)
                {
                    imgpool[i].SetPixel(x, y, img.GetPixel(x+ ix *w -1 ,y+iy*h - 1));
                }
            }
            imgpool[i].Apply();

        }

        

        //cube.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", mainImg);

        gameObjpool = new GameObject[size, size];
        int cnt = 1;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                objcopy = GameObject.Instantiate(cube);
                objcopy.name = cnt.ToString();
                objcopy.transform.position = new Vector3(j, 0, -i);
                gameObjpool[i, j] = objcopy;

                mytext = objcopy.transform.GetChild(0).GetComponent<TextMeshPro>();
                mytext.text = cnt.ToString();//(i*size+j+1).ToString();

                objcopy.GetComponent<MeshRenderer>().material.mainTexture = imgpool[cnt - 1];

                cnt = cnt + 1;    
            }
        }


        StartCoroutine(randPool());
    }
    int ci, cj;
    IEnumerator randPool()
    {
        ci = size - 1;
        cj = size - 1;
        vect = new int[4];
        int n = 10;//打亂次數
        for (int t = 0; t < n; t++)//控制打亂次數
        {
            yield return new WaitForSeconds(0.05f);
            //--------------
            if (ci > 0)
            {
                vect[0] =int.Parse(gameObjpool[ci - 1, cj].name);//上
            }
            else
            {
                vect[0] = 99999;
            }
            //--------------
            if (ci < (size - 1))
            {
                vect[1] = int.Parse(gameObjpool[ci + 1, cj].name);//下
            }
            else
            {
                vect[1] = 99999;
            }
            //--------------
            if (cj > 0)
            {
                vect[2] = int.Parse(gameObjpool[ci, cj - 1].name);//左
            }
            else
            {
                vect[2] = 99999;
            }

            //--------------
            if (cj < (size - 1))
            {
                vect[3] = int.Parse(gameObjpool[ci, cj + 1].name);//右
            }
            else
            {
                vect[3] = 99999;
            }


            int r;
            while (true)
            {
                r = Random.Range(0, 4);
                if (vect[r] != 99999)
                {
                    break;
                }
            }

            ti = ci;
            tj = cj;//ti,tj是我們的目標座標
            //r==0上，r==1下, r=2左, r=3右;
            if (r == 0)
            {
                ti = ci - 1;
            }
            else if (r == 1)
            {
                ti = ci + 1;
            }
            else if (r == 2)
            {
                tj = cj - 1;
            }
            else if (r == 3)
            {
                tj = cj + 1;
            }

            swap();
            //存陣列變數
        }

        gameObjpool[ci, cj].SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
       

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 100);
            
            if (hits.Length > 0)
            {
                print(hits[0].collider.name);
                print(hits[0].collider.gameObject.transform.position);


                ti = (int)-hits[0].collider.gameObject.transform.position.z;
                tj = (int)hits[0].collider.gameObject.transform.position.x;


                print(ti.ToString() + "," + tj.ToString());

                bool check=false;
               
                if (ti > 0) { 
                    print(gameObjpool[ti - 1, tj].name);
                    if (gameObjpool[ti - 1, tj] == gameObjpool[ci, cj])
                    {                       
                        check = true;                        
                    }
                }

                if (ti < (size - 1))
                {
                    print(gameObjpool[ti + 1, tj].name);
                    if (gameObjpool[ti + 1, tj] == gameObjpool[ci, cj])
                    {
                      
                        check = true;                        
                    }
                }
                if (tj > 0) { 
                    print(gameObjpool[ti , tj-1].name);
                    if (gameObjpool[ti, tj-1] == gameObjpool[ci, cj])
                    {
                       
                        check = true;                       
                    }
                }
                if(tj < (size - 1)) { 
                    print(gameObjpool[ti , tj+1].name);
                    if (gameObjpool[ti, tj + 1] == gameObjpool[ci, cj])
                    {
                        check = true;
                        
                    }
                }

              
                if (check == true)
                {
                    swap();
                    int cnt = 1;
                    bool win = true;
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {

                            if ( int.Parse( gameObjpool[i, j].name) != cnt)
                            {
                                win = false;
                                break;
                            }
                            cnt = cnt + 1;
                        }
                    }
                    if(win == true)
                    {
                        exit.SetActive(true);
                    }
                    print(win);

                }

            }
        }

        //print(hits.Length);
    }

    void swap()
    {
        GameObject gameObjecttemp = gameObjpool[ti, tj];
        gameObjpool[ti, tj] = gameObjpool[ci, cj];
        gameObjpool[ci, cj] = gameObjecttemp;//物件陣列

        Vector3 pos1 = gameObjpool[ti, tj].transform.position;
        Vector3 pos2 = gameObjpool[ci, cj].transform.position;
        gameObjpool[ti, tj].transform.position = pos2;
        gameObjpool[ci, cj].transform.position = pos1;
        
        

        ci = ti;
        cj = tj;//更新目前的座標位置
        //record.Push(new int[] { ti, tj }); 本來是想透過堆疊紀錄交換狀況，但結果不管怎麼樣都是顯示空值
        //print(new int[] { ti, tj });
    }
   
}
