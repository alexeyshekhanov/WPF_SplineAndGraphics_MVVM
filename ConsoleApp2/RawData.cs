using ConsoleApp2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp21
{
    public class RawData: IEnumerable<string>
    {
        public double leftLimitOfSegment;
        public double rightLimitOfSegment;
        public int numberOfNodes;
        public bool isUniform;
        public FRaw function;
        public double[] nodesOfGrid;
        public double[] valuesInNodes;

        public RawData(double leftLimitOfSegment, double rightLimitOfSegment, int numberOfNodes, bool isUniform, FRaw function)
        {
            this.leftLimitOfSegment = leftLimitOfSegment;
            this.rightLimitOfSegment = rightLimitOfSegment;
            this.numberOfNodes = numberOfNodes;
            this.isUniform = isUniform;
            this.function = function;
            nodesOfGrid = new double[numberOfNodes];
            valuesInNodes = new double[numberOfNodes];
            
        }
        
        public RawData()
        {
            //this.function = function;
            nodesOfGrid = new double[numberOfNodes];
            valuesInNodes = new double[numberOfNodes];
        }

        public RawData(string fileName)
        {
            var obj = new RawData();
            Load(fileName, ref obj);
            this.leftLimitOfSegment = obj.leftLimitOfSegment;
            this.rightLimitOfSegment = obj.rightLimitOfSegment;
            this.numberOfNodes = obj.numberOfNodes;
            this.isUniform = obj.isUniform;
            this.function = obj.function;
            this.nodesOfGrid = new double[obj.numberOfNodes];
            this.valuesInNodes = new double[obj.numberOfNodes];
            //for (int i = 0; i < obj.numberOfNodes; i++)
            //{
            //    this.nodesOfGrid[i] = obj.nodesOfGrid[i];
            //    this.valuesInNodes[i] = obj.valuesInNodes[i];
            //}
        }

        public void initializeTheValues()
        {
            nodesOfGrid[0] = leftLimitOfSegment;
            nodesOfGrid[numberOfNodes - 1] = rightLimitOfSegment;
            if (isUniform)
            {
                var stepOfTheGrid = (rightLimitOfSegment - leftLimitOfSegment) / (numberOfNodes - 1);
                for (int i = 1; i < numberOfNodes - 1; i++)
                {
                    nodesOfGrid[i] = leftLimitOfSegment + stepOfTheGrid * i;
                }
            }
            else
            {
                var rnd = new Random();
                for (int i = 1; i < numberOfNodes - 1; i++)
                {
                    nodesOfGrid[i] = rightLimitOfSegment - rnd.NextDouble() * (rightLimitOfSegment - leftLimitOfSegment);
                }
            }

            for (int i = 0; i < numberOfNodes; i++)
            {
                valuesInNodes[i] = function(nodesOfGrid[i]); 
            }

            if (!isUniform)
            {
                for (int i = 0; i < nodesOfGrid.Length; i++)
                {
                    for (int j = 0; j < nodesOfGrid.Length - 1; j++)
                    {
                        if (nodesOfGrid[j] > nodesOfGrid[j + 1])
                        {
                            double z = nodesOfGrid[j];
                            nodesOfGrid[j] = nodesOfGrid[j + 1];
                            nodesOfGrid[j + 1] = z;

                            z = valuesInNodes[j];
                            valuesInNodes[j] = valuesInNodes[j + 1];
                            valuesInNodes[j + 1] = z;
                        }
                    }
                }
            }
        }

        public bool Save(string filename)
        {
            var file = new FileStream(filename, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(file);
            try
            {
                writer.Write(leftLimitOfSegment);
                writer.Write(rightLimitOfSegment);
                writer.Write(numberOfNodes);
                //throw new IOException("mess");
                writer.Write(isUniform);
                var temp = "cube";
                if (function == Functions.cube)
                    temp = "cube";
                else if (function == Functions.linear)
                    temp = "linear";
                else if (function == Functions.random)
                    temp = "random";
                writer.Write(temp);
                return true;
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: THE STREAM ERROR");
                return false;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("ERROR: STRING IS NULL");
                return false;
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("ERROR: THE STREAM HAS BEEN CLOSED");
                return false;
            }
            catch
            {
                Console.WriteLine("ERROR: UNEXPECTED ERROR WITH WRITING IN FILE");
                return false;
            }
            finally
            {
                writer.Dispose();
                file.Dispose();
            }
        }

        static public void Load(string filename, ref RawData rawData)
        {
            FileStream file = null;
            BinaryReader reader = null;
            try
            {
                file = new FileStream(filename, FileMode.Open);
                reader = new BinaryReader(file);
                rawData.leftLimitOfSegment = reader.ReadDouble();
                rawData.rightLimitOfSegment = reader.ReadDouble();
                rawData.numberOfNodes = reader.ReadInt32();
                rawData.isUniform = reader.ReadBoolean();
                var temp = reader.ReadString();
                if (temp == "cube")
                    rawData.function = Functions.cube;
                else if (temp == "linear")
                    rawData.function = Functions.linear;
                else if (temp == "random")
                    rawData.function = Functions.random;
                rawData.nodesOfGrid = new double[rawData.numberOfNodes];
                rawData.valuesInNodes = new double[rawData.numberOfNodes];
            }
            catch (EndOfStreamException)
            {
                //Console.WriteLine("ERROR: THE STREAM IS ENDED");
                throw new Exception("ERROR: THE STREAM IS ENDED");
               //return false;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("ERROR: THE NAME OF THE FILE IS INVALID");
                //return false;
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("ERROR: THE STREAM HAS BEEN CLOSED");
               //return false;
            }
            catch (IOException)
            {
                Console.WriteLine("ERROR: THE STREAM ERROR");
               //return false;
            }
            catch
            {
                Console.WriteLine("ERROR: UNEXPECTED ERROR WITH READING FROM FILE");
                //return false;
            }
            finally
            {
                if (file != null)
                {
                    file.Dispose();
                }
                if (reader != null)
                    reader.Dispose();
            }
        }
        public string ToLongString(string format)
        {
            string res = $"Left limit of range is {string.Format(format, leftLimitOfSegment)}, " + "\n" +
                $"Right limit of range is {string.Format(format, rightLimitOfSegment)} " + "\n" +
                $"Number of nodes is  {numberOfNodes}" + "\n" +
                $"Parameter of uniform is {isUniform}" + "\n" +
                $"Function is {function.Method}" + "\n" +
                "    Coordinates and values: " + "\n";
            for (int i = 0; i < this.numberOfNodes; i++)
            {
                res = res + $"    {i}:" + $"{string.Format(format, this.nodesOfGrid[i])}, " + $"{string.Format(format, this.valuesInNodes[i])}" + "\n";
            }
            return res;
        }

        public IEnumerator<string> GetEnumerator()
        {
            var listRawData = new List<string>();
            for (int i = 0; i < nodesOfGrid.Length; i++)
            {
                listRawData.Add(string.Format("{0:0.000}", nodesOfGrid[i]) + " " + string.Format("{0:0.000}", valuesInNodes[i]));
            }

            return listRawData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
