using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetStudio.P5X
{
    public class SelectQuery
    {
        private string[]? SelectedColumns;
        public Type Table;
        public Condition? WhereCond;
        private SelectQuery(Type table, string[]? selectedColumns, Condition? condition)
        {
            Table = table;
            SelectedColumns = selectedColumns;
            WhereCond = condition;
        }
        public string GetSelectedColumns()
        {
            if (SelectedColumns == null) return "*";

            string columns = "";
            for (int i = 0; i < SelectedColumns.Length; i++)
            {
                columns += SelectedColumns[i];
                if (i < SelectedColumns.Length - 1) columns += ", ";
            }
            return columns;
        }
        public string GetWhereCondition()
        {
            if (WhereCond == null) return "";
            else return $" WHERE {WhereCond.ToSQL()}";
        }
        public override string ToString() => $"SELECT {GetSelectedColumns()} FROM {Table.Name}{GetWhereCondition()};";
        public int GetColumnCount() => (SelectedColumns == null) ? Table.GetFields().Length : SelectedColumns.Length;
        public static SelectQuery CreateQuery(Type table, string[]? columns = null, Condition? cond = null)
        {
            var tableMarkerInterface = table.GetInterfaces().Where(x => x.Name == "IDBTableMarker");
            // Type must implement IDBTableMarker
            if (tableMarkerInterface.Count() == 0) throw new Exception($"Type \"{table.Name}\" does not implement interface IDBTableMarker");
            // Verify that all selected columns exist in type
            if (columns != null)
            {
                foreach (var column in columns)
                {
                    if (table.GetFields().Where(x => x.Name == column).Count() == 0)
                        throw new Exception($"Type \"{table.Name}\" does not contain a column named \"{column}\"");
                }
            }
            // Verify that where condition is for a valid column
            if (cond != null)
            {
                if (table.GetFields().Where(x => x.Name == cond.Name).Count() == 0)
                    throw new Exception($"Type \"{table.Name}\" does not contain a column named \"{cond.Name}\"");
            }
            return new SelectQuery(table, columns, cond);
        }
    }
    public abstract class Condition // WHERE x z y
    {
        public string Name { get; init; }
        public string Check { get; init; }
        public abstract string GetOperator();
        public virtual string GetCheck()
        {
            // wrap in quotations for non integer types 
            if (!int.TryParse(Check, out _)) return $"\"{Check}\"";
            return Check;
        }
        public Condition(string name, string check)
        {
            Name = name;
            Check = check;
        }

        public string ToSQL() => $"{Name} {GetOperator()} {GetCheck()}";
    }
    public class WhereOpEqual : Condition
    {
        public WhereOpEqual(string name, string check) : base(name, check) { }
        public override string GetOperator() => "=";
    }
    public class WhereOpGreater : Condition
    {
        public WhereOpGreater(string name, string check) : base(name, check) { }
        public override string GetOperator() => ">";
    }
    public class WhereOpLess : Condition
    {
        public WhereOpLess(string name, string check) : base(name, check) { }
        public override string GetOperator() => "<";
    }
    public abstract class WhereOpLike : Condition
    {
        public WhereOpLike(string name, string check) : base(name, check) { }
        public override string GetOperator() => "LIKE";
    }
    public class WhereOpBegin : WhereOpLike
    {
        public WhereOpBegin(string name, string check) : base(name, check) { }
        public override string GetCheck() => $"\"{Check}%\"";
    }
    public class WhereOpEnd : WhereOpLike
    {
        public WhereOpEnd(string name, string check) : base(name, check) { }
        public override string GetCheck() => $"\"%{Check}\"";
    }
    public class WhereOpContains : WhereOpLike
    {
        public WhereOpContains(string name, string check) : base(name, check) { }
        public override string GetCheck() => $"\"%{Check}%\"";
    }
}
