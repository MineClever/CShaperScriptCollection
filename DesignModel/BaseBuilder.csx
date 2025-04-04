using System;
using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        // 将顶级语句移到 Main 方法中
        UserScript.ScriptRunner.RunDefaultScript();
    }
}

public abstract class InterfaceBuilder
{
    public abstract void BuildA(int count = 1);
    public abstract void BuildB(int count = 1);
    public abstract void BuildC(int count = 1);
    public abstract void Reset();
    public abstract object GetResult();
}

public class ProductionABuilder : InterfaceBuilder
{
    private ProductionA _production;

    public ProductionABuilder()
    {
        _production = new ProductionA();
    }

    public override void Reset()
    {
        _production = new ProductionA();
    }

    public override void BuildA(int count = 1)
    {
        _production.Add($"ProductionA_a".PadRight(count + 11, 'a'));
    }

    public override void BuildB(int count = 1)
    {
        _production.Add($"ProductionA_b".PadRight(count + 11, 'b'));
    }

    public override void BuildC(int count = 1)
    {
        _production.Add($"ProductionA_c".PadRight(count + 11, 'c'));
    }

    public override object GetResult()
    {
        var result = _production;
        Reset();
        return result;
    }
}

public class ProductionA
{
    private List<string> _productionContent = new List<string>();

    public void Add(string partString)
    {
        _productionContent.Add("Add: " + partString);
    }

    public void OpenProduction()
    {
        Console.WriteLine(string.Join(", ", _productionContent));
    }
}

public class ProductionBBuilder : InterfaceBuilder
{
    private ProductionB _production;

    public ProductionBBuilder()
    {
        _production = new ProductionB();
    }

    public override void Reset()
    {
        _production = new ProductionB();
    }

    public override void BuildA(int count = 1)
    {
        _production.Append($"ProductionB_a".PadRight(count + 11, 'a'));
    }

    public override void BuildB(int count = 1)
    {
        _production.Append($"ProductionB_b".PadRight(count + 11, 'b'));
    }

    public override void BuildC(int count = 1)
    {
        _production.Append($"ProductionB_c".PadRight(count + 11, 'c'));
    }

    public override object GetResult()
    {
        var result = _production;
        Reset();
        return result;
    }
}

public class ProductionB
{
    private List<string> _productionContent = new List<string>();

    public void Append(string partString)
    {
        _productionContent.Add("Append: " + partString);
    }

    public void UseProduction()
    {
        Console.WriteLine(string.Join(", ", _productionContent));
    }
}

public class Director
{
    private InterfaceBuilder _builder;

    public InterfaceBuilder Builder
    {
        get => _builder;
        set => _builder = value;
    }

    public void BuildSimple()
    {
        if (_builder == null)
            throw new Exception("No valid builder");

        _builder.BuildA();
    }

    public void BuildHard()
    {
        if (_builder == null)
            throw new Exception("No valid builder");

        _builder.BuildA(2);
        _builder.BuildC(2);
        _builder.BuildB(3);
    }
}

public class Client
{
    private Director _director;

    public void SetDirector(Director director)
    {
        _director = director;
    }

    public void AskSimpleBuild(InterfaceBuilder builder)
    {
        if (_director == null)
            throw new Exception("No valid director");

        _director.Builder = builder;
        _director.BuildSimple();
    }

    public void AskHardBuild(InterfaceBuilder builder)
    {
        if (_director == null)
            throw new Exception("No valid director");

        _director.Builder = builder;
        _director.BuildHard();
    }

    public void SelfManageComplexBuilder(InterfaceBuilder builder)
    {
        Console.WriteLine($"The Client: I'm asking {builder.GetType().Name} to build a complex thing!");
        builder.BuildA(10);
        builder.BuildB(12);
        builder.BuildA(3);
        builder.BuildC(4);
    }
}

namespace UserScript
{
    public static class ScriptRunner
    {
        public static void RunDefaultScript()
        {
            var client = new Client();
            var director = new Director();
            client.SetDirector(director);

            var builderA = new ProductionABuilder();
            var builderB = new ProductionBBuilder();

            client.AskSimpleBuild(builderA);
            var productionASimple = (ProductionA)builderA.GetResult();
            productionASimple.OpenProduction();

            client.AskHardBuild(builderA);
            var productionAHard = (ProductionA)builderA.GetResult();
            productionAHard.OpenProduction();

            client.AskHardBuild(builderB);
            var productionBHard = (ProductionB)builderB.GetResult();
            productionBHard.UseProduction();

            client.SelfManageComplexBuilder(builderB);
            var productionBComplex = (ProductionB)builderB.GetResult();
            productionBComplex.UseProduction();

            client.SelfManageComplexBuilder(builderA);
            var productionAComplex = (ProductionA)builderA.GetResult();
            productionAComplex.OpenProduction();
        }
    }
}

