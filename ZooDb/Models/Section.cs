using System.ComponentModel.DataAnnotations;

public class Section
{
    [Key]
    public int Id{get;set;}
    public string Name{get;set;}
    public List<Animal> Animals {get;set;} = new List<Animal>();
}