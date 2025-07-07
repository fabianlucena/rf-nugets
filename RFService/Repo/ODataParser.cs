using Sprache;

static class ODataParser
{
    public abstract record Nodo;
    public record Comparation(string Field, string Operator, string Value) : Nodo;
    public record Function(string Name, string Field, string Argument) : Nodo;
    public record Binary(string Operator, Nodo Left, Nodo Right) : Nodo; static readonly Parser<string> Identifier =
        Parse.Letter.AtLeastOnce().Text();

    static readonly Parser<string> Operator =
        Parse.String("eq")
            .Or(Parse.String("ne"))
            .Or(Parse.String("gt"))
            .Or(Parse.String("lt"))
            .Or(Parse.String("ge"))
            .Or(Parse.String("le"))
            .Text();

    static readonly Parser<string> Text =
        from open in Parse.Char('\'')
        from content in Parse.CharExcept('\'').Many().Text()
        from close in Parse.Char('\'')
        select content;

    static readonly Parser<Nodo> SimpleComparation =
        from field in Identifier.Token()
        from op in Operator.Token()
        from value in Text.Or(Identifier).Token()
        select new Comparation(field, op, value);

    static readonly Parser<Nodo> SimpleFunction =
        from name in Identifier
        from open in Parse.Char('(')
        from field in Identifier.Token()
        from coma in Parse.Char(',').Token()
        from argument in Text
        from close in Parse.Char(')')
        select new Function(name, field, argument);

    static readonly Parser<Nodo> Term =
        SimpleFunction.Or(SimpleComparation).Token();

    static readonly Parser<Nodo> Expression =
        Parse.ChainOperator(
            Parse.String("and").Or(Parse.String("or")).Token(),
            Term,
            (op, left, right) => new Binary(new string([.. op]), left, right)
        );

    public static Nodo ParseOData(string input) => Expression.End().Parse(input);
}