import 'bootstrap/dist/css/bootstrap.min.css';
import { Table } from 'react-bootstrap';
import { FaUserPlus } from 'react-icons/fa';
import { useNavigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import axios from 'axios';

export default function StudentTable() {
  const navigate = useNavigate();
  const [students, setStudents] = useState([]);

  useEffect(() => {
    axios.get('http://localhost:5038/api/Students') // update base URL as needed
      .then((res) => {
        setStudents(res.data);
        console.log(res.data); // Moved inside the .then block
      })
      .catch((err) => console.error(err));
  }, []);

  const handleAddStudent = () => {
    navigate('/student/create');
  };

  const handleEdit = (id) => {
    navigate(`/student/edit/${id}`);
  };

  const handleView = (id) => {
    navigate(`/student/view/${id}`);
  };

  const handleDelete = (id) => {
    if (window.confirm('Are you sure to delete this student?')) {
      axios.delete(`http://localhost:5038/api/Students/${id}`)
        .then(() => setStudents(students.filter(s => s.id !== id)))
        .catch(err => console.error(err));
    }
  };

  return (
    <div className="container mt-5">
      <div className="card shadow-lg border-0">
        <div className="card-header bg-primary text-white text-center py-4">
          <h2 className="mb-0 fw-bold">🎓 Student Records</h2>
        </div>
        <div className="card-body text-start">
          <button className="btn btn-secondary btn-lg px-4 py-2 mb-4" onClick={handleAddStudent}>
            <FaUserPlus className="me-2" />
            Add New Student
          </button>

          <Table striped bordered hover className="align-middle text-center shadow-sm">
            <thead className="table-primary">
              <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Place</th>
                <th>Phone</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {students.map((student) => (        
                <tr key={student.id}>
                  <td>{student.id}</td>
                  <td>{student.name}</td>
                  <td>{student.place}</td>
                  <td>{student.phone}</td>
                  <td>
                    <button className="btn btn-sm btn-info me-2" onClick={() => handleView(student.id)}>View</button>
                    <button className="btn btn-sm btn-warning me-2" onClick={() => handleEdit(student.id)}>Edit</button>
                    <button className="btn btn-sm btn-danger" onClick={() => handleDelete(student.id)}>Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </Table>
        </div>
      </div>
    </div>
  );
}
