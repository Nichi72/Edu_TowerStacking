using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mammal mammal = new Mammal();
        mammal.Nurse();

        Dog dog = new Dog();
        dog.Nurse();
        dog.Bark();

        Cat cat = new Cat();
        cat.Nurse();
        cat.Meow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MammalCastTest()
    {
        Mammal mammal = new Mammal();
        mammal.Nurse();

        Dog dogTest = new Dog();

        mammal = dogTest;

        mammal = new Dog();
        mammal.Nurse();
        Dog dog = (Dog)mammal;
        dog.Nurse();
        dog.Bark();
        

        mammal = new Cat();
        mammal.Nurse();

        Cat cat = (Cat)mammal;
        cat.Nurse();
        cat.Meow();

        Test123 test123_A = new Test123();
        Test123 test123_B = new Test123();

        Test123 test1232 = new Test123("테스트" , 7272);
        Test123 test1233 = new Test123("테스트2" , 444);


        // test123.intTest
        // test123.nameTest
    }
}
class Test123
{
    public string nameTest;
    public int intTest;
    public Test123()
    {
        intTest = 123;
        nameTest = "test123";
    }
    public Test123(string nameTest , int intTest)
    {
        this.nameTest = nameTest;
        this.intTest = intTest;
    }
}

class Mammal 
{
    public void Nurse() { Debug.Log("Nurse"); } 
}
class Dog : Mammal 
{
    public void Bark() { Debug.Log("Bark"); } 
}
class Cat : Mammal 
{
    public void Meow() { Debug.Log("Meow"); }
}


