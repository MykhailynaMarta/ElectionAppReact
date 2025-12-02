import './App.css'
import Home from './component/Home'
import Candidates from './component/Candidates'
import CandidatePage from './component/CandidatePage'
import AddCandidate from './component/AddCandidate'
import UpdateCandidate from './component/UpdateCandidate'
import LoginUploadUserFile from './component/Authorization/LoginUploadUserFile'
import CallbackPage from './component/Authorization/CallbackPage'
import Navbar from './component/Navbar';
import UserInfo from './component/UserInfo';
import { Routes, Route } from 'react-router-dom';



function App() {

  return (
      <>
          <Navbar />
          <Routes>
              <Route path="/upload" element={<LoginUploadUserFile />} />
              <Route path="/user-info" element={<UserInfo />} />
              <Route path="/callback" element={<CallbackPage />} />
              <Route path="/candidates" element={<Candidates />} />
              <Route path="/candidates/add" element={<AddCandidate />} />
              <Route path="/candidates/edit/:id" element={<UpdateCandidate />} />
              <Route path="/candidate/:id" element={<CandidatePage />} />
              <Route path="/" element={<Home />} />
          </Routes>
    </>
  )
}

export default App
