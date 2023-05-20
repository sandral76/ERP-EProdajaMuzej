using System.Linq.Expressions;

namespace Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public BaseSpecification(Expression<Func<T, bool>> criteria, Expression<Func<T, bool>> criteriay)
        {
            Criteria = criteria;
            Criteriay = criteriay;
        }
        public Expression<Func<T, bool>> Criteria { get; }
        public Expression<Func<T, bool>> Criteriay { get; }

        public List<Expression<Func<T, object>>> Includes { get; } =
        new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected void AddInclude(Expression<Func<T, object>> includeExpression, Expression<Func<T, object>> includeExpressiony)
        {
            Includes.Add(includeExpression);
            Includes.Add(includeExpressiony);
        }
        protected void AddInclude(Expression<Func<T, object>> includeExpression, Expression<Func<T, object>> includeExpressiony, Expression<Func<T, object>> includeExpressionz)
        {
            Includes.Add(includeExpression);
            Includes.Add(includeExpressiony);
            Includes.Add(includeExpressionz);
        }
        

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}