using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour
{
    [SerializeField] private CityDisplaySettings displaySettings;
    // В этом сценарии написать функцую вызывающую окно в GUI, а в отдельном сценарии для города прописать реакцию на клик со ссылкой на функцию в этом сценарии
    // Сценарий этот прикрепить через сериализацию к экземпляру города
    // Start is called before the first frame update
    void Start()
    {
        displaySettings.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            displaySettings.Open();
        }    
    }
}
