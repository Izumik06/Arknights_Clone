using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SqlClient;

namespace Izumik
{
    public class test : MonoBehaviour
    {
        string connectString = "Server=localhost;Database=test;uid=sa;pwd=1234;";
        // Start is called before the first frame update
        void Start()
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                //Select로 받아오면 ExcuteReader, 다른 sql는 ExcuteQuery
                SqlDataReader dr = new SqlCommand("Select name from [Table]", conn).ExecuteReader();
                dr.Read();
                string value = dr.GetString(0);
                Debug.Log(value);

                dr.Close();
            } 
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
