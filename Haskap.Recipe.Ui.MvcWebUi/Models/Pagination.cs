namespace Haskap.Recipe.Ui.MvcWebUi.Models;

public class Pagination
{
    private const int _defaultPageSize = 10;

    public int PageSize { get; }
    public int CurrentPageIndex { get; } // starts from 1
    public int ItemCount { get; }
    public int PageCount { get; }
    public bool HasNextPage => (PageCount > 1 && CurrentPageIndex > 0 && CurrentPageIndex < PageCount);
    public bool HasPreviousPage => (PageCount > 1 && CurrentPageIndex > 0 && CurrentPageIndex > 1);
    
    public Pagination(int pageSize, int currentPageIndex, int itemCount)
    {
        PageSize=pageSize < 1 ? _defaultPageSize : pageSize;
        ItemCount=itemCount<0 ? 0 : itemCount;
        PageCount = (int)Math.Ceiling(ItemCount / (double)PageSize);
        CurrentPageIndex=currentPageIndex < 1 || currentPageIndex > PageCount || pageSize < 1 ? 0 : currentPageIndex;
    }

    public int GetFirstPageIndex(int visiblePageIndexCount)
    {
        if (CurrentPageIndex < 1)
        {
            return 1;
        }

        if (visiblePageIndexCount < 1)
        {
            visiblePageIndexCount = 1;
        }
        var pageIndexCountBeforeCurrentPageIndex = CurrentPageIndex == PageCount ? visiblePageIndexCount - 1 : visiblePageIndexCount - 2;
        if (pageIndexCountBeforeCurrentPageIndex < 0)
        {
            pageIndexCountBeforeCurrentPageIndex = 0;
        }
        
        return CurrentPageIndex - pageIndexCountBeforeCurrentPageIndex < 1 ? 1 : CurrentPageIndex - pageIndexCountBeforeCurrentPageIndex;
    }

    public int GetLastPageIndex(int visiblePageIndexCount)
    {
        if (CurrentPageIndex < 1)
        {
            return 1;
        }

        if (visiblePageIndexCount < 1)
        {
            visiblePageIndexCount = 1;
        }
        var pageIndexCountAfterCurrentPageIndex = CurrentPageIndex == 1 ? visiblePageIndexCount - 1 : visiblePageIndexCount - 2;
        if (pageIndexCountAfterCurrentPageIndex < 0)
        {
            pageIndexCountAfterCurrentPageIndex = 0;
        }

        return CurrentPageIndex + pageIndexCountAfterCurrentPageIndex > PageCount ? PageCount : CurrentPageIndex + pageIndexCountAfterCurrentPageIndex;
    }

    
}
