namespace AgendaProject.Models
{
    public partial class AgendaItem
    {
        public int MoveUp()
        {
            var sectionHeading = db.SectionHeadings.Find(this.SectionHeadingId);


            switch (sectionHeading.AgendaItems.Count)
            {
                case 2:
                    // there are two items, move the second one to the first position.
                    var firstAgendaItem = db.AgendaItems.Find(Prev);
                    var secondAgendaItem = db.AgendaItems.Find(Id);
                    firstAgendaItem.Prev = firstAgendaItem.Nxt;
                    firstAgendaItem.Nxt = null;
                    secondAgendaItem.Nxt = Prev;
                    secondAgendaItem.Prev = null;

                    db.SaveChanges();

                    break;
                case 3:
                    // there are three items, deal with all three
                    // you won't be moving the first one up, so its either item 2 or three moving
                    if (this.Nxt == null) // then its item three moving up
                    {
                        var secondItem = db.AgendaItems.Find(Prev);
                        var firstItem = db.AgendaItems.Find(secondItem.Prev);
                        var thisItem = db.AgendaItems.Find(Id);

                        firstItem.Nxt = this.Id;

                        thisItem.Prev = firstItem.Id;
                        thisItem.Nxt = secondItem.Id;

                        secondItem.Prev = this.Id;
                        secondItem.Nxt = null;


                        db.SaveChanges();
                    }
                    else
                    {
                        // its the second Item moving up
                        var itemOne = db.AgendaItems.Find(Prev);
                        var itemThree = db.AgendaItems.Find(Nxt);
                        var itemTwo = db.AgendaItems.Find(Id);

                        itemTwo.Prev = null;
                        itemTwo.Nxt = itemOne.Id;

                        itemOne.Prev = Id;
                        itemOne.Nxt = itemThree.Id;

                        itemThree.Prev = itemOne.Id;

                        db.SaveChanges();
                    }
                    break;
                default:
                    // there are four or more rows.
                    // if its the bottom moving up, there is no childNode
                    // if its row 2 moving up, there is no parentNode
                    // otherwise, its the full blown case.

                    // we have the targetNode:  this.
                    // Flow:
                    /*  
                        Get the previous node:  set default flag.
                        test for top case, or get the parentNode, set the top flag
                        test for bottom case or get the childNode, set the bottom flag
                        perform the flag actions.                           
                    */
                    string caseFlag = "";
                    var previousNode = db.AgendaItems.Find(Prev);
                    var thisNode = db.AgendaItems.Find(Id);
                    var parentNode = new AgendaItem();
                    var childNode = new AgendaItem();

                    if (previousNode.Prev != null)
                    {
                        parentNode = db.AgendaItems.Find(previousNode.Prev);
                    }
                    else
                    {
                        caseFlag = "top";
                    }
                    if (Nxt != null)
                    {
                        childNode = db.AgendaItems.Find(Nxt);
                    }
                    else
                    {
                        caseFlag = "bottom";
                    }

                    switch (caseFlag)
                    {
                        case "top":
                            // no parent node
                            thisNode.Prev = null;
                            thisNode.Nxt = previousNode.Id;

                            previousNode.Prev = thisNode.Id;
                            previousNode.Nxt = childNode.Id;

                            childNode.Prev = previousNode.Id;
                            db.SaveChanges();
                            break;
                        case "bottom":
                            // there is no child Node
                            parentNode.Nxt = thisNode.Id;

                            thisNode.Prev = parentNode.Id;
                            thisNode.Nxt = previousNode.Id;

                            previousNode.Prev = thisNode.Id;
                            previousNode.Nxt = null;
                            db.SaveChanges();
                            break;
                        default:
                            parentNode.Nxt = thisNode.Id;

                            thisNode.Prev = parentNode.Id;
                            thisNode.Nxt = previousNode.Id;

                            previousNode.Prev = thisNode.Id;
                            previousNode.Nxt = childNode.Id;

                            childNode.Prev = previousNode.Id;
                            db.SaveChanges();
                            break;
                    }

                    break;
            }

            return sectionHeading.AgendaId;
        }
    }
}