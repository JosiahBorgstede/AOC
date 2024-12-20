[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class AOCAttribute : Attribute {
    private int part;

    private string name;

    private string? description;
    public int Part {get{return part;}}

    public string Name {get {return name;}}

    public AOCAttribute(int part, string name) {
        this.part = part;
        this.name = name;
    }

    public string? Description {get {return description;} set {this.description = value;}}

}