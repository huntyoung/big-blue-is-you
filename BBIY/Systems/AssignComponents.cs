using Microsoft.Xna.Framework;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Systems
{
    class AssignComponents : System
    {
        private List<Entity> m_babaEntities;
        private List<Entity> m_flagEntities;
        private List<Entity> m_lavaEntities;
        private List<Entity> m_rockEntities;
        private List<Entity> m_wallEntities;
        private List<Entity> m_waterEntities;

        private Action<Entity> m_addEntity;
        private Action<Entity> m_removeEntity;
        private Action<char, int, int> m_checkAndAdd;

        private List<Entity> m_removeMe;

        public AssignComponents(Action<Entity> addEntity, Action<Entity> removeEntity, Action<char, int, int> checkAndAdd) 
            : base(typeof(Components.ChangeableObject))
        {
            m_addEntity = addEntity;
            m_removeEntity = removeEntity;
            m_checkAndAdd = checkAndAdd;

            m_removeMe = new List<Entity>();

            m_babaEntities = new List<Entity>();
            m_flagEntities = new List<Entity>();
            m_lavaEntities = new List<Entity>();
            m_rockEntities = new List<Entity>();
            m_wallEntities = new List<Entity>();
            m_waterEntities = new List<Entity>();
        }

        public bool addToEntityList(Entity entity)
        {
            if (!entity.ContainsComponent<Components.ChangeableObject>()) return false;

            var changeableObject = entity.GetComponent<Components.ChangeableObject>();
            string objectType = changeableObject.objectType;
            
            List<Entity> objectList = getEntityList(objectType);
            if (!objectList.Contains(entity))
            {
                objectList.Add(entity);
                return true;
            }
            return false;
        }

        public bool removeFromEntityList(Entity entity)
        {
            if (!entity.ContainsComponent<Components.ChangeableObject>()) return false;

            var changeableObject = entity.GetComponent<Components.ChangeableObject>();
            string objectType = changeableObject.objectType;

            List<Entity> objectList = getEntityList(objectType);
            if (objectList.Contains(entity))
            {
                objectList.Remove(entity);
                return true;
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            List<string> newRules = SetRules.rulesList.Except(SetRules.previousRulesList).ToList();
            List<string> oldRules = SetRules.previousRulesList.Except(SetRules.rulesList).ToList();

            if (newRules.Count > 0 || oldRules.Count > 0) SetRules.previousRulesList = new List<string>(SetRules.rulesList);

            foreach (string rule in oldRules)
            {
                Debug.WriteLine("Old Rule: " + rule);

                string[] words = rule.Split(' ');
                try
                {
                    List<Entity> objectList = getEntityList(words[0]);
                    if (objectList == null) throw new Exception("First word in rule is not a noun");

                    if (words[1] != "is") throw new Exception("Second word in rule must be a verb/linking word");

                    Components.Component oldComponent = getComponent(words[2]);

                    foreach (var entity in objectList)
                    {
                        removeComponentFromEntity(entity, oldComponent);
                    }
                }
                catch (Exception e) { Debug.WriteLine(e); }
            }

            foreach (string rule in newRules)
            {
                Debug.WriteLine("New Rule: " + rule);

                string[] words = rule.Split(' ');
                try
                {
                    List<Entity> objectList = getEntityList(words[0]);
                    if (objectList == null) throw new Exception("First word in rule is not a noun");

                    if (words[1] != "is") throw new Exception("Second word in rule must be a verb/linking word");

                    foreach (var entity in objectList)
                    {
                        // if third word is discriptive word
                        Components.Component newComponent = getComponent(words[2]);
                        if (newComponent != null) addComponentToEntity(entity, newComponent);
                        else
                        {
                            // if third word is noun
                            char entityChar = getEntityChar(words[2]);
                            if (entityChar == '0') Debug.WriteLine("Something went terribly wrong...");
                            else replaceEntity(entity, entityChar);
                        }
                    }

                    foreach (var entity in m_removeMe)
                    {
                        removeFromEntityList(entity);
                    }
                    m_removeMe.Clear();
                }
                catch (Exception e) { Debug.WriteLine(e); }
            }
        }

        private void addComponentToEntity(Entity entity, Components.Component newComponent)
        {
            m_removeEntity(entity);
            entity.Add(newComponent);
            //// add entity again to systems because it may be a part of new systems with changed components
            m_addEntity(entity);
        }

        private void removeComponentFromEntity(Entity entity, Components.Component oldComponent)
        {
            m_removeEntity(entity);
            entity.Remove(oldComponent);
            //// add entity again to systems because it may be a part of new systems with changed components
            m_addEntity(entity);
        }

        private List<Entity> getEntityList(string word)
        {
            switch (word)
            {
                case "baba":
                    return m_babaEntities;
                case "flag":
                    return m_flagEntities;
                case "lava":
                    return m_lavaEntities;
                case "rock":
                    return m_rockEntities;
                case "wall":
                    return m_wallEntities;
                case "water":
                    return m_waterEntities;
                case null:
                    throw new NullReferenceException("First word in rule is null");
                default:
                    return null;
            }
        }

        private Components.Component getComponent(string word)
        {
            switch (word)
            {
                case "you":
                    return new Components.IsYou();
                case "win":
                    return new Components.IsWin();
                case "stop":
                    return new Components.IsStop();
                case "push":
                    return new Components.IsPush();
                case "sink":
                    return new Components.IsSink();
                case "kill":
                    return new Components.IsKill();
                case null:
                    throw new NullReferenceException("Third word in rule is null");
                default:
                    return null;
            }
        }

        private char getEntityChar(string word)
        {
            switch (word)
            {
                case "baba":
                    return 'b';
                case "flag":
                    return 'f';
                case "lava":
                    return 'v';
                case "rock":
                    return 'r';
                case "wall":
                    return 'w';
                case "water":
                    return 'a';
                case null:
                    throw new NullReferenceException("Word given is null");
                default:
                    return '0';
            }
        }


        private void replaceEntity(Entity oldEntity, char newEntityChar)
        {
            var position = oldEntity.GetComponent<Components.Position>();
            m_removeMe.Add(oldEntity);
            m_removeEntity(oldEntity);

            m_checkAndAdd(newEntityChar, position.x, position.y);
        }
    }
}
