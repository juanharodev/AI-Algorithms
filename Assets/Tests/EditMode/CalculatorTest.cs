using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CalculatorTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void CalculatorAddTest()
    {
        //Arrange
        Calculadora calculadora = new Calculadora();

        //Assert
        int resultado = calculadora.Add(10,9);
        Assert.AreEqual(19, resultado);
    }

    [Test]
    public void CalculatorMultiplyTest()
    {
        //Arrange
        Calculadora calculadora = new Calculadora();

        //Assert
        int resultado = calculadora.Multiply(10, 9);
        Assert.AreEqual(90, resultado);
    }
}
