[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class AOCAttribute : Attribute {
    private int part;
    private int day;

    private string name;

    private string? description;
    public int Part {get{return part;}}
    public int Day {get {return day;}}

    public string Name {get {return name;}}

    public AOCAttribute(int part, int day, string name) {
        this.part = part;
        this.day = day;
        this.name = name;
    }

    public string? Description {get {return description;} set {this.description = value;}}

    

}