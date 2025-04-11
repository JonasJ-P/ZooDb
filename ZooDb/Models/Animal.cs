using System.ComponentModel.DataAnnotations;

public class Animal
{
    [Key]
    public int Id{get;set;}
    public string Name{get;set;}
    public string Type{get;set;}
    public int Age{get;set;}

}