using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood, //0
    Stone, //1
    Ore, //2
    Clay, //3
    Glass, //4
    Paper, //5
    Textile, //6
    Money, //7
    Point, //8
    ScienceCompas, //9
    ScienceStone, //10
    ScienceGear, //11
    MilitaryShield,//12
    Marvel, //13
    RESOURCE_COUNT = 14
}


public class Option<T>
{
    [SerializeField]
    public List<T> content;
};
[System.Serializable]
public class OptionResource : Option<ResourceType> {
    public ResourceList contentAsList;
};
[System.Serializable]
public class ResourceList : IEnumerable<ResourceType>
{
  
    public int[] content = new int[(int)ResourceType.RESOURCE_COUNT];

    public int this[ResourceType r]
    {
        get { return content[(int)r]; }
        set { content[(int)r] = value; }
    }
    override public string ToString()
    {
        string result = "[";
        for (int i = 0; i < (int)ResourceType.RESOURCE_COUNT; ++i)
        {
            result += this[(ResourceType)i];
            if (i < ((int)ResourceType.RESOURCE_COUNT) - 1)
            {
                result += ",";
            }
        }
        result += "]";
        return result;
    }
    IEnumerator<ResourceType> GetEnumerator()
    {
        for(int i = 0; i < (int)ResourceType.RESOURCE_COUNT; ++i)
        {
            if (content[i] ==0)
            {
                continue;
            }
            yield return (ResourceType)i;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    IEnumerator<ResourceType> IEnumerable<ResourceType>.GetEnumerator()
    {
        return GetEnumerator();
    }

    public ResourceList Copy()
    {
        var result = new ResourceList();
        content.CopyTo(result.content,0);
        return result;
    }
    public void Add(ResourceType resource, int amount = 1)
    {
        /*if (!content.ContainsKey(resource))
        {
            content[(int)resource] = amount;
        }
        else*/
        {
            content[(int)resource] += amount;
        }
    }
    public int Get(ResourceType resource)
    {
        /*if (!content.ContainsKey(resource))
        {
            return 0;
        }
        else*/
        {
            return content[(int)resource];
        }
    }
    public int Remove(ResourceType resource, int amount = 1)
    {
        /*if (!content.ContainsKey(resource))
        {
            return 0;
        }
        else*/
        {
            int available = content[(int)resource];
            if(available> amount)
            {
                content[(int)resource] -= amount;
                return amount;
            }
            else
            {
                content[(int)resource] = 0;
                return available;
            }
            
        }
    }
    public int RemoveAll(ResourceType resource)
    {
      
        /*if (!content.ContainsKey(resource))
        {
            return 0;
        }
        else*/
        {
            int result = content[(int)resource];
            content[(int)resource] = 0;
            return result;
        }
    }

}
public class Cost : ResourceList{ 

}
public interface IProductor
{
    OptionResource[] GetProduction();
}
public class Production {

    public List<IProductor> productors;
    public List<OptionResource[]> options;
    public List<ResourceList> GetAllProductionAlternatives()
    {
       List<ResourceList> result = new List<ResourceList>();
       // sort all productors, the ones with more options at the end
       for(int i = options.Count -2; i >= 0; --i)
        {
            var option = options[i];
            if (option.Length > 1)
            {
                options.RemoveAt(i);
                options.Add(option);
            }
        }
        result.Add(new ResourceList());
        // create a graph
        for (int i =0; i < options.Count; ++i)
        {
            var option = options[i];
            if (option.Length == 1)
            {
                foreach(var current in result)
                {
                    foreach (var resource in option[0].content)
                    {
                        current[resource] += 1;
                    }
                }
            }
            else
            {
                var addition = new List<ResourceList>();
                foreach (var current in result)
                {
                    foreach (var opt in options)
                    {
                        var newResult = current.Copy();
                        foreach (var resource in opt[0].content)
                        {
                            newResult[resource] += 1;
                        }
                        addition.Add(newResult);
                    }
                }
                result.Clear();
                result.AddRange(addition);
            }
        }
        foreach(var r in result)
        {
            Debug.Log(r.ToString());
        }

        return result;
    }
}

public class Resource : ScriptableObject
{
    public ResourceType type;


    public static Color[] resourceColor = new Color[13]
    {
        new Color(0.5f,0,0,1), //Wood
        new Color(0.5f,0.5f,0.5f,1),// Stone,
        new Color(0.5f,0.5f,0f,1),//Ore, 
        new Color(1f,0.5f,0,1),//Clay,
        new Color(0,0.25f,1f,1),//Glass,
        new Color(1,1,1f,1),//Paper, 
        new Color(1,0,1,1),//Textile,
        new Color(1,1,0f,1), //Money, 
        new Color(0,1,0,1),//Point, 
        new Color(0.25f,1,0.25f,1),//ScienceCompas, 
        new Color(0.25f,1,0.25f,1),//ScienceStone,
        new Color(0.25f,1,0.25f,1),//ScienceGear, 
        new Color(1,0,0,1), //MilitaryShield
    };
}
