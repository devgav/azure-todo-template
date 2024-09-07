import axios from 'axios';
import { useState, useEffect } from 'react';
import { Button, Card, Form, ListGroup } from 'react-bootstrap'
import { Trash } from 'react-bootstrap-icons'

type TodoItem = {
  task: string;
  id?: number;
  isCompleted: boolean;
}

const API_URI = 'http://localhost:5140/api/Todo'

function App() {
  const [data, setData] = useState<TodoItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);
  const [formInput, setFormInput] = useState('');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(API_URI);
        setData(response.data);
      } catch (error) {
        setError(false);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>There was some error.</p>;

  const handleAddTodo = async (event: React.MouseEvent<HTMLButtonElement>) => {
    event.preventDefault();
    const todoItem: TodoItem = {
      task: formInput,
      isCompleted: false, 
    }
    const result = await axios.post(API_URI, todoItem);
    if (result.status >= 200) {
      setData(prev => [...prev, result.data]);
      setFormInput('');
    }
  }
  const handleUpdateTodo = async (d: TodoItem) => {
    d.isCompleted = true;
    const result = await axios.put(`${API_URI}/${d.id}`, d);
    if (result.status >= 200) {
      setData(prev => prev.filter((item) => item.id !== d.id));
    }
  }
  const handleDeleteTodo = async (id: number) => {
    const result = await axios.delete(`${API_URI}/${id}`);
    if (result.status >= 200) {
      setData(prev => prev.filter((item) => item.id !== id));
    }
  }

  return (
    <div className='d-flex justify-content-center align-items-center vh-100'>
      <Card className='text-center' style={{width: '400px'}}>
        <Card.Body>
          <Card.Title>Todo Application</Card.Title>
          <Form>
            <Form.Group controlId='formNewTodo' className='d-flex'>
              <Form.Control type="text" placeholder="Enter your todo" value={formInput} onChange={(e) => setFormInput(e.target.value)}/>
              <Button type="submit" style={{margin: '5px'}} variant="primary" onClick={(e) => handleAddTodo(e)}>Add</Button>
            </Form.Group>
          </Form>
          <ListGroup className="mt-3">
              <h5>Your Todos:</h5>
              {
                data.map((d) => (
                  <ListGroup.Item key={`todo-${d.id}`} className='d-flex justify-content-between'>
                    <Form.Check
                      type="checkbox"
                      label={d.task}
                      onChange={() => handleUpdateTodo(d)}
                    />
                    <div>
                      <Trash color='red' title='delete' size='25' onClick={() => handleDeleteTodo(d.id!)}/>
                    </div>
                </ListGroup.Item>
                ))
              }
          </ListGroup>
        </Card.Body>
      </Card>
    </div>
  )
}

export default App
