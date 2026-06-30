import { useParams, useNavigate } from "react-router-dom";
import { courses } from "../data/courses";

export default function CourseDetails() {
  const { courseId } = useParams();
  const navigate = useNavigate();
  const course = courses.find((c) => c.id === courseId);

  if (!course) {
    return (
      <section className="page">
        <h1>Course not found</h1>
        <button className="btn btn--primary" onClick={() => navigate("/courses")}>
          Back to Courses
        </button>
      </section>
    );
  }

  return (
    <section className="page">
      <p className="eyebrow">Course Details</p>
      <h1>{course.title}</h1>

      <dl className="detail-list">
        <div>
          <dt>Course ID</dt>
          <dd>{course.id}</dd>
        </div>
        <div>
          <dt>Category</dt>
          <dd>{course.category}</dd>
        </div>
        <div>
          <dt>Duration</dt>
          <dd>{course.duration}</dd>
        </div>
        <div>
          <dt>Trainer</dt>
          <dd>{course.trainer}</dd>
        </div>
        <div>
          <dt>Description</dt>
          <dd>{course.description}</dd>
        </div>
      </dl>

      <button className="btn btn--primary" onClick={() => navigate("/courses")}>
        Back to Courses
      </button>
    </section>
  );
}
