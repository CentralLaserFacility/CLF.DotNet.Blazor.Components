//
// ScenariosCollection.cs
//

using System.Collections.Generic;

namespace Clf.LogicSystem
{

  // 'Name' chould be a record type that puts constraints
  // on the string, eg single line, no spaces, no special characters ...

  public record ScenariosCollection ( string Name )
  {

    public ScenariosCollection (
      string            collectionName,
      params Scenario[] scenariosInCollection
    ) : 
    this(collectionName)
    {
      m_scenariosInCollection.AddRange(
        scenariosInCollection
      ) ;
    }

    public ScenariosCollection (
      string                collectionName,
      IEnumerable<Scenario> scenariosInCollection
    ) : 
    this(
      Name : collectionName
    ) {
      m_scenariosInCollection.AddRange(
        scenariosInCollection
      ) ;
    }

    private List<Scenario> m_scenariosInCollection = new() ;

    public IEnumerable<Scenario> AvailableScenarios => m_scenariosInCollection ;

    public void Add ( Scenario scenarioToAdd ) => m_scenariosInCollection.Add(scenarioToAdd) ;

  }

}
