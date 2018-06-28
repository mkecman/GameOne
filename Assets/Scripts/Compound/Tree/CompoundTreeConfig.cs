using System.Collections.Generic;

public class CompoundTreeConfig : TreeBranchData
{
    public TreeBranchData GetBranch( int index, List<TreeBranchData> children = null )
    {
        if( this.Index == index )
            return this;

        if( children == null )
            children = this.Children;

        TreeBranchData branch = null;
        foreach( TreeBranchData child in children )
        {
            if( child.Index == index )
                return child;
            if( child.Children.Count > 0 )
            {
                branch = GetBranch( index, child.Children );
                if( branch != null )
                    return branch;
            }
        }

        return branch;
    }
}
