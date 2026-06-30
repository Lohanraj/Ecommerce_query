import { useNavigate } from "react-router-dom";
import { courses } from "../data/courses";

export default function Courses() {
  const navigate = useNavigate();

  return (
    <section className="page">
      <p className="eyebrow">Catalog</p>
      <h1>Available Courses</h1>

      <div className="course-grid">
        {courses.map((course) => (
          <article className="course-card" key={course.id}>
            <h3>{course.title}</h3>
            <dl className="course-card__meta">
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
            </dl>
            <button
              className="btn btn--primary btn--block"
              onClick={() => navigate(`/courses/${course.id}`)}
            >
              View Details
            </button>
          </article>
        ))}
      </div>
    </section>
  );
}
