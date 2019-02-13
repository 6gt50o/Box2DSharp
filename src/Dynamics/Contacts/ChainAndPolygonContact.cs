using System.Diagnostics;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts
{
    public class ChainAndPolygonContact : Contact
    {
        internal override void Evaluate(ref Manifold manifold, in Transform xfA, Transform xfB)
        {
            var chain = (ChainShape) FixtureA.Shape;

            chain.GetChildEdge(out var edge, ChildIndexA);
            CollisionUtils.CollideEdgeAndPolygon(ref manifold, edge, xfA, (PolygonShape) FixtureB.Shape, xfB);
        }
    }

    internal class ChainAndPolygonContactFactory : IContactFactory
    {
        private readonly ObjectPool<ChainAndPolygonContact> _pool =
            new ObjectPool<ChainAndPolygonContact>(new ContactPoolPolicy<ChainAndPolygonContact>());

        public Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
        {
            Debug.Assert(fixtureA.ShapeType == ShapeType.Chain);
            Debug.Assert(fixtureB.ShapeType == ShapeType.Polygon);
            var contact = _pool.Get();
            contact.Initialize(fixtureA, indexA, fixtureB, indexB);
            return contact;
        }

        public void Destroy(Contact contact)
        {
            _pool.Return((ChainAndPolygonContact) contact);
        }
    }
}